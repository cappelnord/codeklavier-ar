using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LBublonisGenerator : LGenerator
{

    public Material BranchMaterial;
    public GameObject JointPrefab;

    private LSystemController lsysController;
    private WedgeMeshGen wedgeMeshGen;


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        wedgeMeshGen = WedgeMeshGen.Instance();
        lsysController = LSystemController.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        Grow(transform, lsys.Units[0], 0, 0.5f, 0.06f, 30.0f, 0.3f);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastLengthUp, float lastBaseRadius, float lastBendAngle, float lastJointScale)
    {
        int childrenIndex = -1;

        float jointScale = lastJointScale * 0.9f;

        if (generation == 0)
        {
            /*
            GameObject joint = Instantiate(JointPrefab, parent);

            LifeBehaviour lbs = joint.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lbs.TargetScale = new Vector3(jointScale * 0.5f, jointScale * 0.5f, jointScale * 0.5f);
            lbs.GrowStartTime = Time.time;

            FlowBehaviour fbs = joint.AddComponent<FlowBehaviour>() as FlowBehaviour;
            fbs.Gen = this;
            */

        }

        foreach (ProcessUnit unit in children)
        {
            childrenIndex++;

            if (!VisitUnit(unit) || unit.Content == '0')
            {
                GameObject endPoint = Instantiate(JointPrefab, parent);
                endPoint.transform.localPosition = new Vector3(0.0f, lastLengthUp, 0.0f);

                LifeBehaviour lbe = endPoint.AddComponent<LifeBehaviour>() as LifeBehaviour;
                lbe.TargetScale = new Vector3(jointScale * 0.5f, jointScale, jointScale);
                lbe.GrowStartTime = Time.time;

                BublonisBubbleIntensity bbi = endPoint.AddComponent<BublonisBubbleIntensity>() as BublonisBubbleIntensity;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);

                continue;
            }

            float thisLengthUp = lastLengthUp * 0.9f;
            float thisBaseRadius = lastBaseRadius * 0.7f;
            float thisBendAngle = lastBendAngle *0.9f; // should probably depend on number of generations in this object

            // TODO: in case there is only 1 axiom we need to terminate it gracefully on the bottom

            GameObject obj = Spawn(parent, unit, thisLengthUp, thisBaseRadius, lastBaseRadius);


            if (generation == 0)
            {
                obj.transform.localEulerAngles = LPrunastriGenerator.StartAngles[children.Count - 1][childrenIndex];
            }
            else
            {
                obj.transform.localPosition = new Vector3(0.0f, lastLengthUp, 0.0f);
                obj.transform.localEulerAngles = new Vector3(0.0f, (float)childrenIndex / children.Count * 360.0f, thisBendAngle);
            }

            LifeBehaviour lb = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lb.TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
            lb.GrowStartTime = Time.time + (generation * 0.5f);

            FlowBehaviour fb = obj.AddComponent<FlowBehaviour>() as FlowBehaviour;
            fb.RotationMultiplier = 20f;
            fb.Gen = this;

            IntensityBehaviour ib = obj.AddComponent<BublonisBranchIntensity>() as IntensityBehaviour;
            ib.Gen = this;




            if (unit.Children.Count == 0)
            {
                GameObject endPoint = Instantiate(JointPrefab, obj.transform);
                endPoint.transform.localPosition = new Vector3(0.0f, lastLengthUp, 0.0f);

                LifeBehaviour lb2 = endPoint.AddComponent<LifeBehaviour>() as LifeBehaviour;
                lb2.TargetScale = new Vector3(jointScale * 0.5f, jointScale, jointScale);
                lb2.GrowStartTime = Time.time + (generation * 0.5f);

                BublonisBubbleIntensity bbi = endPoint.AddComponent<BublonisBubbleIntensity>() as BublonisBubbleIntensity;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);

            }

            Grow(obj.transform, unit.Children, generation + 1, thisLengthUp, thisBaseRadius, thisBendAngle, jointScale);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, float lengthUp, float baseRadius, float lastBaseRadius)
    {
        const int sides = 16;

        float radiusCenter = baseRadius * 1.5f;
        float radiusUp = baseRadius;
        float radiusDown = baseRadius * 0.45f;
        float lengthDown = 0.1f * lengthUp;
        float squish = 0.4f;
        float squishUp = squish;
        // put this to extreme to have thorns
        float squishDown = squish * 3.0f;

        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);
    }

}
