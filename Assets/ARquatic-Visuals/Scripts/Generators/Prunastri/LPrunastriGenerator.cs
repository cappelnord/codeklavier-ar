using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


namespace ARquatic.Visuals {
public class LPrunastriGenerator : LGenerator
{

    public Material BranchMaterial;
    public Material FruitMaterial;

    private LSystemController lsysController;
    private WedgeMeshGen wedgeMeshGen;

    private Color branchGrey;
    private Color branchGreen;
    private Color fruitGreen;

    private Material[] jointMaterials = new Material[10];
    private Material branchMaterialCopy;
    private bool didAddBranchIntensity = false;

    static private List<List<Vector3>> StartAngles =
    new List<List<Vector3>>() {
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f) },
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 180.0f) },
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(45.0f, -45.0f, 180.0f), new Vector3(45.0f, 135.0f, 180.0f) },
        new List<Vector3>() {new Vector3(0.0f, 0.0f, 0.0f), new Vector3(0.0f, 0.0f, 180.0f), new Vector3(0.0f, 180.0f, 0.0f), new Vector3(0.0f, 180.0f, 180.0f) }
    };


    static public Vector3 GetStartAngle(int childrenCount, int index)
    {
        List<Vector3> list = LPrunastriGenerator.StartAngles[(childrenCount - 1) % 4];
        Vector3 angle = list[index % list.Count];

        if(index >= 4)
        {
            angle = Quaternion.Euler(0f, 0f, 90f) * angle;
        }

        return angle;
    }


    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        // branchGrey = new Color(0.8f, 0.8f, 0.8f);

        if(!Variety)
        {
            branchGreen = new Color((60f / 255f) * 0.8f, (168f / 255f) * 0.8f, (19f / 255f) * 0.8f);
            fruitGreen = new Color((80f / 255f) * 0.8f, (198f / 255f) * 0.8f, (29f / 255f) * 0.8f);
            branchGrey = new Color((60f / 255f) * 1.0f, (168f / 255f) * 0.9f, (19f / 255f) * 0.9f);
        } else
        {
            fruitGreen = new Color((40f / 255f) * 0.8f, (168f / 255f) * 0.8f, (39f / 255f) * 0.8f);
            branchGreen = new Color((50f / 255f) * 0.8f, (198f / 255f) * 0.8f, (49f / 255f) * 0.8f);
            branchGrey = new Color((60f / 255f) * 1.0f, (168f / 255f) * 0.9f, (19f / 255f) * 0.9f);
        }


        wedgeMeshGen = WedgeMeshGen.Instance();
        lsysController = LSystemController.Instance();

        SpeciesVelocityMultiplier = 1.25f;

    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        if (IsEmpty()) return;

        for(int i = 0; i < 10; i++) {
            jointMaterials[i] = Instantiate(FruitMaterial);
        }

        didAddBranchIntensity = true;
        branchMaterialCopy = Instantiate(BranchMaterial);

        float startAngle = 25f;
        float startRadius = 0.02f + (lsys.Units.Count * 0.002f);
        if(Variety)
        {
            startAngle = 10f;
            startRadius *= 0.8f;
        }

        Grow(transform, lsys.Units[0], 0, 0.25f, startRadius, startAngle);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastLengthUp, float lastBaseRadius, float lastBendAngle)
    {
        float baseLengthUp = 1.25f;
        if(Variety)
        {
            baseLengthUp = 1.35f;
        }

        float lengthUpMult = baseLengthUp - (lsys.RecursionDepth * 0.05f);
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
            float thisBendAngle = lastBendAngle * bendAngleMult * SymbolDynamicsMultiplier(unit.Dynamic, 0.5f); // should probably depend on number of generations in this object

            // TODO: in case there is only 1 axiom we need to terminate it gracefully on the bottom

            Material mat = branchMaterialCopy;

            if(unit.Children.Count == 0)
            {
                int materialIndex = (int) char.GetNumericValue(unit.Content);
                mat = jointMaterials[materialIndex];
            }

            GameObject obj = Spawn(parent, unit, thisLengthUp, thisBaseRadius, lastBaseRadius, null);
            obj.GetComponent<MeshRenderer>().sharedMaterial = mat;


            if(generation == 0)
            {
                obj.transform.localEulerAngles = LPrunastriGenerator.GetStartAngle(children.Count, childrenIndex);
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
                if(!didAddBranchIntensity) {
                    PrunastriBranchIntensity ib = obj.AddComponent<PrunastriBranchIntensity>() as PrunastriBranchIntensity;
                    ib.Gen = this;
                    ib.Green = branchGreen;
                    ib.Grey = branchGrey;
                    didAddBranchIntensity = true;
                }

            } else
            {
                PrunastriFruitIntensity bbi = obj.AddComponent<PrunastriFruitIntensity>() as PrunastriFruitIntensity;
                bbi.Gen = this;
                bbi.KeyColor = materialLookup.GetColor(unit.Content);
                bbi.Green = fruitGreen;

                lbe.TargetScale = new Vector3(1.0f, 1.0f, 1.0f) * SymbolDynamicsMultiplier(unit.Dynamic);
            }

            Grow(obj.transform, unit.Children, generation + 1, thisLengthUp, thisBaseRadius, thisBendAngle);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, float lengthUp, float baseRadius, float lastBaseRadius, Material mat)
    {
        const int sides = 16;

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
        return wedgeMeshGen.GetWedgeObject(sides, radiusCenter, radiusUp, lengthUp, radiusDown, lengthDown, squish, squishUp, squishDown, parent, mat);
    }

}
}
