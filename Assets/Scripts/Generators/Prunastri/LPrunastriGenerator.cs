using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LPrunastriGenerator : LGenerator
{

    public Material BranchMaterial;

    private LSystemController lsysController;
    private WedgeMeshGen wedgeMeshGen;

    private Color branchGrey;
    private Color branchGreen;
    private Color fruitGreen;

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

        branchGrey = new Color(0.8f, 0.8f, 0.8f);

        if(!Variety)
        {
            branchGreen = new Color((60f / 255f) * 0.8f, (168f / 255f) * 0.8f, (19f / 255f) * 0.8f);
            fruitGreen = new Color((80f / 255f) * 0.8f, (198f / 255f) * 0.8f, (29f / 255f) * 0.8f);
        } else
        {
            fruitGreen = new Color((40f / 255f) * 0.8f, (168f / 255f) * 0.8f, (39f / 255f) * 0.8f);
            branchGreen = new Color((50f / 255f) * 0.8f, (198f / 255f) * 0.8f, (49f / 255f) * 0.8f);
        }


        wedgeMeshGen = WedgeMeshGen.Instance();
        lsysController = LSystemController.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        float startAngle = 20f;
        float startRadius = 0.03f + (lsys.Units.Count * 0.01f);
        if(Variety)
        {
            startAngle = 10f;
            startRadius *= 0.5f;
        }

        Grow(transform, lsys.Units[0], 0, 0.25f, startRadius, startAngle);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastLengthUp, float lastBaseRadius, float lastBendAngle)
    {
        float lengthUpMult = 1.4f - (lsys.RecursionDepth * 0.05f);
        float bendAngleMult = 1.4f - (lsys.RecursionDepth * 0.04f);
        float baseRadiusMult = 0.5f + (lsys.RecursionDepth * 0.04f);

        if(Variety)
        {
            baseRadiusMult = baseRadiusMult * 0.85f;
        }

        int childrenIndex = -1;
        foreach (ProcessUnit unit in children)
        {
            childrenIndex++;

            if (!VisitUnit(unit) || unit.Content == '0')
            {
                continue;
            }

            float thisLengthUp = lastLengthUp * lengthUpMult;
            float thisBaseRadius = lastBaseRadius * baseRadiusMult;
            float thisBendAngle = lastBendAngle * bendAngleMult; // should probably depend on number of generations in this object

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
                PrunastriBranchIntensity ib = obj.AddComponent<PrunastriBranchIntensity>() as PrunastriBranchIntensity;
                ib.Gen = this;
                ib.Green = branchGreen;
                ib.Grey = branchGrey;

            } else
            {
                PrunastriFruitIntensity bbi = obj.AddComponent<PrunastriFruitIntensity>() as PrunastriFruitIntensity;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);
                bbi.Green = fruitGreen;
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

        if (unit.Children.Count == 0)
        {
            radiusUp = baseRadius * 4.0f;
            if(Variety)
            {
                radiusDown *= 2.0f;
            }
            lengthUp = lengthUp * 0.5f;
            squishUp = 0.1f;
        } else
        {
            radiusDown = baseRadius * 0.5f;
            lengthDown = lengthDown * 1.05f;
        }
        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, BranchMaterial);
    }

}
