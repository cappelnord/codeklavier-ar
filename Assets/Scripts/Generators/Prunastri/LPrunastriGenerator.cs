using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LPrunastriGenerator : LGenerator
{

    public Material BranchMaterial;

    private LSystemController lsysController;
    private WedgeMeshGen wedgeMeshGen;

    static public List<List<Vector3>> StartAngles =
    new List<List<Vector3>>() {
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f) },
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 180.0f) },
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(45.0f, -45.0f, 180.0f), new Vector3(45.0f, 135.0f, 180.0f) }
    };



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

        Grow(transform, lsys.Units[0], 0, 0.25f, 0.1f, 20.0f);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastLengthUp, float lastBaseRadius, float lastBendAngle)
    {
        int childrenIndex = -1;
        foreach (ProcessUnit unit in children)
        {
            childrenIndex++;

            if (!VisitUnit(unit) || unit.Content == '0')
            {
                continue;
            }

            float thisLengthUp = lastLengthUp * 1.25f;
            float thisBaseRadius = lastBaseRadius * 0.666f;
            float thisBendAngle = lastBendAngle * 1.3f; // should probably depend on number of generations in this object

            // TODO: in case there is only 1 axiom we need to terminate it gracefully on the bottom

            GameObject obj = Spawn(parent, unit, thisLengthUp, thisBaseRadius, lastBaseRadius);

            if(generation == 0)
            {
                obj.transform.localEulerAngles = LPrunastriGenerator.StartAngles[children.Count-1][childrenIndex];
            }
            else
            {
                obj.transform.localPosition = new Vector3(0.0f, lastLengthUp, 0.0f);
                obj.transform.localEulerAngles = new Vector3(0.0f, (float)childrenIndex / children.Count * 360.0f, thisBendAngle);
            }

            LifeBehaviour lbe = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lbe.TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
            lbe.GrowStartTime = Time.time + (0.5f * generation);

            FlowBehaviour fb = obj.AddComponent<FlowBehaviour>() as FlowBehaviour;
            fb.RotationMultiplier = 20f;
            fb.Gen = this;

            if(unit.Children.Count != 0) {
                IntensityBehaviour ib = obj.AddComponent<PrunastriBranchIntensity>() as IntensityBehaviour;
                ib.Gen = this;
            } else
            {
                IntensityBehaviour bbi = obj.AddComponent<PrunastriFruitIntensity>() as IntensityBehaviour;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);
            }

            Grow(obj.transform, unit.Children, generation + 1, thisLengthUp, thisBaseRadius, thisBendAngle);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, float lengthUp, float baseRadius, float lastBaseRadius)
    {
        const int sides = 12;

        float radiusCenter = lastBaseRadius;
        float radiusUp = baseRadius;
        float radiusDown = 0.0f;
        float lengthDown = 0.05f * lengthUp;
        float squish = 1.0f;
        float squishUp = 1.0f;
        float squishDown = 1.0f;

        if(unit.Children.Count == 0)
        {
            radiusUp = baseRadius * 4.0f;
            lengthUp = lengthUp * 0.5f;
            squishUp = 0.1f;
        }
        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);
    }

}
