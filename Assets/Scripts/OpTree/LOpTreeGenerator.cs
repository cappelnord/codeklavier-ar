using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LOpTreeGenerator : LGenerator
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
    void Start()
    {
        base.Start();

        wedgeMeshGen = WedgeMeshGen.Instance();
        materialLookup = MaterialLookup.Instance();
    }

    override public void Generate()
    {
        ResetRandomness();

        mainSquish = RandomRange(0.2f, 1.0f);
        bottomSquish = RandomRange(0.2f, 1.5f);
        bottomRadius = RandomRange(0.25f, 1.0f);
        radiusMul = RandomRange(0.2f, 1.0f);

        flowerRadius = RandomRange(0.5f, 3.0f);
        flowerSquish = RandomRange(0.5f, 1.5f);
        flowerLength = RandomRange(0.25f, 2.0f);

        growAngle = RandomRange(20.0f, 50.0f);

        foreach (Transform child in transform)
        {
            // Destroy(child.gameObject);
            child.gameObject.GetComponent<GrowBehaviour>().Die();
        }

        Grow(transform, lsys.units[0], 0.98f, 0.0f);
    }

    void Grow(Transform parent, List<ProcessUnit> children, float growWeight, float growDelay)
    {
        if (children.Count == 0) return;

        float angleZ = 0.0f;
        float angleY = 0.0f;
        if (children.Count > 1) angleZ = growAngle;

        float deltaAngleY = 360.0f / children.Count;

        float newGrowWeight = growWeight + (1.0f - growWeight) * 0.2f;

        foreach(ProcessUnit unit in children)
        {
            GameObject obj = Spawn(parent, unit);
            if(obj == null)
            {
                continue;
            }

            /*
            obj.transform.Rotate(0.0f, angleY, angleZ);
            obj.transform.localScale *= 0.8f;
            obj.transform.Translate(0.0f, 3.0f * obj.transform.lossyScale[0], 0.0f);
            */

            GrowBehaviour grow = obj.GetComponent<GrowBehaviour>();
            grow.gen = this;
            grow.targetRotation = new Vector3(0.0f, angleY, angleZ);
            grow.targetScale = new Vector3(0.8f, 0.8f, 0.8f);
            grow.targetPosition = new Vector3(0.0f, 3.0f, 0.0f);
            grow.currentPosition = new Vector3(0.0f, 3.0f, 0.0f);
            grow.growDelay = growDelay;

    

            grow.growWeight = growWeight;

            obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);


            angleY += deltaAngleY;

            Grow(obj.transform, unit.Children, newGrowWeight, growDelay + 0.05f);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit)
    {

        
        if (unit.Content == '0')
        {
            return null;
        }
        

        float upRadius = 0.8f;

        float dynamicFactor = 0.5f + ((float)unit.Dynamic / 127.0f) * 1.0f;

        int sides = 16;

        if (unit.Children.Count == 0)
        {
            if (unit.Content == '6' || unit.Content == '7')
            {
                return wedgeMeshGen.GetWedgeObject(sides, dynamicFactor, 0.0f, 3.0f, 1.0f, 0.5f, 0.01f , 0.2f, bottomSquish, parent, branchMaterial);
            } else
            {
                Material flowerMaterial = materialLookup.Get(unit.Content);
                return wedgeMeshGen.GetWedgeObject(sides, 0.5f, flowerRadius * dynamicFactor, flowerLength, 0.5f, 0.5f, 0.2f, flowerSquish, 0.2f, parent, flowerMaterial);
            }
        } else
        {

            return wedgeMeshGen.GetWedgeObject(sides, 0.5f * radiusMul * dynamicFactor, upRadius * radiusMul * dynamicFactor, 3.0f, bottomRadius * radiusMul * dynamicFactor, 0.5f, mainSquish, 0.5f, bottomSquish, parent, branchMaterial);

        }

    }
}
