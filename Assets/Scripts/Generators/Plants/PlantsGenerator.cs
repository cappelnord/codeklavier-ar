using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.LSystem;
using ARquatic.Visuals;


public class PlantsGenerator : LGenerator
{
    public GameObject ProtoEmpty;
    public Material BranchMaterial;

    private WedgeMeshGen wedgeMeshGen;

    private float mainSquish;
    private float bottomSquish;
    private float bottomRadius;
    private float radiusMul;
    private float flowerRadius;
    private float flowerSquish;
    private float flowerLength;
    private float growAngle;



    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        wedgeMeshGen = WedgeMeshGen.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        mainSquish = RandomRange(0.2f, 1.0f);
        bottomSquish = RandomRange(0.2f, 1.5f) * 0.1f;
        bottomRadius = RandomRange(0.25f, 1.0f) * 0.1f;
        radiusMul = RandomRange(0.2f, 1.0f);

        flowerRadius = RandomRange(0.5f, 3.0f) * 0.1f;
        flowerSquish = RandomRange(0.5f, 1.5f);
        flowerLength = RandomRange(0.25f, 2.0f) * 0.1f;

        growAngle = RandomRange(20.0f, 50.0f);

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<PlantsGrowBehaviour>().Die();
        }

        Grow(transform, lsys.Units[0], 0.98f, 0.0f, 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, float growWeight, float growDelay, int generation)
    {
        if (children.Count == 0) return;

        float angleZ = 0.0f;
        float angleY = 0.0f;
        if (children.Count > 1) angleZ = growAngle;

        float deltaAngleY = 360.0f / children.Count;

        float newGrowWeight = growWeight + (1.0f - growWeight) * 0.2f;

        float initPosition = 0.0f;
        if(generation == 0)
        {
            initPosition = -0.5f;
        }

        foreach(ProcessUnit unit in children)
        {
            if(!VisitUnit(unit) || unit.Content == '0')
            {
                continue;
            }

            GameObject obj = Spawn(parent, unit);

            /*
            obj.transform.Rotate(0.0f, angleY, angleZ);
            obj.transform.localScale *= 0.8f;
            obj.transform.Translate(0.0f, 3.0f * obj.transform.lossyScale[0], 0.0f);
            */

            
            PlantsGrowBehaviour grow = obj.AddComponent<PlantsGrowBehaviour>() as PlantsGrowBehaviour;
            grow.Gen = this;
            grow.TargetRotation = new Vector3(0.0f, angleY, angleZ);
            grow.TargetScale = new Vector3(0.8f, 0.8f, 0.8f);
            grow.TargetPosition = new Vector3(0.0f, 3.0f * 0.1f + initPosition, 0.0f);
            grow.CurrentPosition = new Vector3(0.0f, 3.0f * 0.1f + initPosition, 0.0f);
            grow.GrowDelay = growDelay;

            grow.GrowWeight = growWeight;

            obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);


            angleY += deltaAngleY;

            Grow(obj.transform, unit.Children, newGrowWeight, growDelay + 0.05f, generation+1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit)
    {

        float upRadius = 0.8f * 0.1f;

        float lengthUp = 0.3f;
        float lengthDown = 0.05f;


        float dynamicFactor = 0.5f + ((float)unit.Dynamic / 127.0f) * 1.0f;

        int sides = 16;

        if (unit.Children.Count == 0)
        {
            if (unit.Content == '6' || unit.Content == '7')
            {
                return wedgeMeshGen.GetWedgeObject(sides, dynamicFactor * 0.1f, 0.0f, lengthUp, 1.0f * 0.1f, lengthDown, 0.01f , 0.2f, bottomSquish, parent, BranchMaterial);
            } else
            {
                Material flowerMaterial = materialLookup.Get(unit.Content);
                return wedgeMeshGen.GetWedgeObject(sides, 0.5f * 0.1f, flowerRadius * dynamicFactor, flowerLength, 0.5f * 0.1f, lengthDown, 0.2f, flowerSquish, 0.2f, parent, flowerMaterial);
            }
        } else
        {

            return wedgeMeshGen.GetWedgeObject(sides, 0.5f * 0.1f * radiusMul * dynamicFactor, upRadius * radiusMul * dynamicFactor, lengthUp, bottomRadius * radiusMul * dynamicFactor, lengthDown, mainSquish, 0.5f, bottomSquish, parent, BranchMaterial);

        }

    }
}
