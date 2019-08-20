using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpTreeGenerator : LGenerator
{
    public GameObject protoEmpty;
    public Material branchMaterial;

    private WedgeMeshGen wedgeMeshGen;
    private MaterialLookup materialLookup;

    private float mainSquish;
    private float bottomSquish;
    private float bottomRadius;
    private float radiusMul;
    private float flowerRadius;
    private float flowerSquish;
    private float flowerLength;
    private float growAngle;
    

    // Start is called before the first frame update
    override protected void Start()
    {
        base.Start();

        wedgeMeshGen = WedgeMeshGen.Instance();
        materialLookup = MaterialLookup.Instance();
    }

    override public void Generate()
    {
        ResetRandomness();

        mainSquish = RandomRange(0.8f, 1.0f);
        bottomSquish = RandomRange(0.8f, 1.2f) * 0.2f;
        bottomRadius = RandomRange(0.8f, 1.2f) * 0.2f;
        radiusMul = RandomRange(0.2f, 1.0f);

        flowerRadius = RandomRange(0.5f, 3.0f) * 0.1f;
        flowerSquish = RandomRange(0.5f, 1.5f);
        flowerLength = RandomRange(0.25f, 2.0f) * 0.1f;

        growAngle = RandomRange(-2000.0f, 2000.0f);

        foreach (Transform child in transform)
        {
            // Destroy(child.gameObject);
            child.gameObject.GetComponent<OpTreeGrowBehaviour>().Die();
        }

        Grow(transform, lsys.units[0], 0.98f, 0.0f, 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, float growWeight, float growDelay, int generation)
    {
        if (children.Count == 0) return;

        float angleZ = 0.0f;
        float angleY = 0.0f;
        float angleX = 90.0f;

        if (children.Count > 1) angleZ = growAngle;

        float deltaAngleY = 360.0f / children.Count;

        float newGrowWeight = growWeight + (1.0f - growWeight) * 0.2f;

        float initPosition = 0.0f;
        if (generation == 0)
        {
            initPosition = -0.5f;
        }

        foreach (ProcessUnit unit in children)
        {
            GameObject obj = Spawn(parent, unit, generation);
            if(obj == null)
            {
                continue;
            }

            /*
            obj.transform.Rotate(0.0f, angleY, angleZ);
            obj.transform.localScale *= 0.8f;
            obj.transform.Translate(0.0f, 3.0f * obj.transform.lossyScale[0], 0.0f);
            */



            OpTreeGrowBehaviour grow = obj.AddComponent<OpTreeGrowBehaviour>() as OpTreeGrowBehaviour;
            grow.gen = this;

            float currentGrowMult = 0.9f;
            if (unit.Children.Count == 0)
            {
                currentGrowMult = 1.0f;
                grow.windStrength = 0.0f;
            }

            grow.currentRotation = new Vector3(angleX * currentGrowMult, angleY * currentGrowMult, angleZ * currentGrowMult);
            grow.targetRotation = new Vector3(angleX, angleY, angleZ);
            grow.targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            grow.targetPosition = new Vector3(0.0f, 3.0f * 0.25f + initPosition, 0.0f);
            grow.currentPosition = new Vector3(0.0f, 3.0f * 0.25f + initPosition, 0.0f);
            grow.growDelay = growDelay;

            grow.growWeight = growWeight;

            obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);


            angleY += deltaAngleY;

            Grow(obj.transform, unit.Children, newGrowWeight, growDelay + 0.05f, generation+1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, int generation)
    {

        
        if (unit.Content == '0')
        {
            return null;
        }
        

        float upRadius = 0.8f * 0.1f;

        float lengthUp = 0.5f + (0.1f * generation);
        float lengthDown = 0.05f;


        float dynamicFactor = 0.5f + ((float)unit.Dynamic / 127.0f) * 1.0f;

        int sides = 32;

        if (unit.Children.Count == 0)
        {
            
            Material flowerMaterial = materialLookup.Get(unit.Content);
            return wedgeMeshGen.GetWedgeObject(sides, 0.5f, flowerRadius * dynamicFactor, flowerLength, 0.5f, lengthDown, flowerSquish, flowerSquish, flowerSquish, parent, flowerMaterial);
        
        } else
        {

            // GetWedgeObject(int sides, float radiusCenter, float radiusUp, float lengthUp, float radiusDown, float lengthDown, float squish, float squishUp, float squishDown, Transform transform, Material material)

            return wedgeMeshGen.GetWedgeObject(sides, 0.5f * 0.1f * radiusMul * dynamicFactor, upRadius * radiusMul * dynamicFactor, lengthUp, bottomRadius * radiusMul * dynamicFactor, lengthDown, mainSquish, bottomSquish, bottomSquish, parent, branchMaterial);

        }

    }
}
