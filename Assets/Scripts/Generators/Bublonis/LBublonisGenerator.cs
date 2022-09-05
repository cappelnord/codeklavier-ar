using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LBublonisGenerator : LGenerator
{

    public Material BranchMaterial;
    public GameObject JointPrefab;

    private LSystemController lsysController;
    private WedgeMeshGen wedgeMeshGen;

    private Color branchGrey;
    private Color branchGreen;
    private Color bubbleGreen;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // branchGrey = new Color(0.8f, 0.8f, 0.8f);
        if (!Variety)
        {
            branchGreen = new Color((30f / 255f) * 0.8f, (120f / 255f) * 0.8f, (10f / 255f) * 0.8f);
            bubbleGreen = new Color((60f / 255f) * 0.8f, (168f / 255f) * 0.8f, (19f / 255f) * 0.8f);
            branchGrey = new Color((30f / 255f) * 1.0f, (120f / 255f) * 0.9f, (10f / 255f) * 0.9f);
        }
        else
        {
            branchGreen = new Color((60f / 255f) * 0.8f, (130f / 255f) * 0.8f, (10f / 255f) * 0.8f);
            bubbleGreen = new Color((80f / 255f) * 0.8f, (168f / 255f) * 0.8f, (19f / 255f) * 0.8f);
            branchGrey = new Color((60f / 255f) * 1.0f, (130f / 255f) * 0.9f, (10f / 255f) * 0.9f);
        }

        wedgeMeshGen = WedgeMeshGen.Instance();
        lsysController = LSystemController.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        if (!IsEmpty())
        {
            Grow(transform, lsys.Units[0], 0, 0.5f, 0.06f, 30.0f, 0.3f);
        }
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastLengthUp, float lastBaseRadius, float lastBendAngle, float lastJointScale)
    {
        int childrenIndex = -1;

        float jointScale = lastJointScale * 0.85f;

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
                bbi.Green = bubbleGreen;

                if (Variety)
                {
                    lbe.TargetScale = lbe.TargetScale * Random.Range(0.45f, 0.6f);
                }

                continue;
            }

            float thisLengthUp = lastLengthUp * 0.9f;
            float thisBaseRadius = lastBaseRadius * 0.7f;
            float thisBendAngle = lastBendAngle *0.9f; // should probably depend on number of generations in this object

            // TODO: in case there is only 1 axiom we need to terminate it gracefully on the bottom

            GameObject obj = Spawn(parent, unit, thisLengthUp, thisBaseRadius, lastBaseRadius, generation == 0);


            if (generation == 0)
            {
                obj.transform.localEulerAngles = LPrunastriGenerator.GetStartAngle(children.Count, childrenIndex);
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

            BublonisBranchIntensity ib = obj.AddComponent<BublonisBranchIntensity>() as BublonisBranchIntensity;
            ib.Gen = this;
            ib.Green = branchGreen;
            ib.Grey = branchGrey;




            if (unit.Children.Count == 0)
            {
                GameObject endPoint = Instantiate(JointPrefab, obj.transform);
                endPoint.transform.localPosition = new Vector3(0.0f, lastLengthUp, 0.0f);

                LifeBehaviour lb2 = endPoint.AddComponent<LifeBehaviour>() as LifeBehaviour;
                lb2.TargetScale = new Vector3(jointScale * 0.5f, jointScale, jointScale) * SymbolDynamicsMultiplier(unit.Dynamic, 0.1f);

                lb2.GrowStartTime = Time.time + (generation * 0.5f);

                BublonisBubbleIntensity bbi = endPoint.AddComponent<BublonisBubbleIntensity>() as BublonisBubbleIntensity;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);
                bbi.Green = bubbleGreen;

            }

            Grow(obj.transform, unit.Children, generation + 1, thisLengthUp, thisBaseRadius, thisBendAngle, jointScale);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, float lengthUp, float baseRadius, float lastBaseRadius, bool isFirst)
    {
        int sides = 16;

        float radiusCenter = baseRadius * 1.5f;
        float radiusUp = baseRadius;
        float radiusDown = baseRadius * 0.45f;
        float lengthDown = 0.1f * lengthUp;
        float squish = 0.4f;
        float squishUp = squish;
        // put this to extreme to have thorns
        
        float squishDown = squish * 3.0f;

        if(Variety)
        {
            radiusCenter = baseRadius;
            if (!isFirst)
            {
                squishDown = squish * 30f;
            }
            squishUp = 1.0f;
        }

        if(isFirst)
        {
            radiusDown = radiusDown * 0.25f;
        }

        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);
    }

}
