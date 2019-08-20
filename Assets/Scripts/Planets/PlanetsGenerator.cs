using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsGenerator : LGenerator
{

    public GameObject protoPlanet;
    public GameObject protoEmpty;

    override protected void Start()
    {
        base.Start();
    }

    override public void Generate()
    {
        ResetRandomness();

        foreach (Transform child in transform)
        {
            // Destroy(child.gameObject);
            PlanetsBehaviour behave = child.gameObject.GetComponent<PlanetsBehaviour>();
            if(behave != null)
            {
                behave.Die();
            }
        }

        Grow(gameObject.transform, lsys.units[0], 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation)
    {
        float outwardRadius = 1.0f - (generation * RandomRange(0.02f, 0.08f));
        // add an empty in center if not-singular axiom
        if(generation == 0 && children.Count == 1) {
            outwardRadius = 0.0f;
        }

        float phase = RandomRange(0.0f, 360.0f);
        float angle = 0.0f;
        float deltaAngle = 360.0f / children.Count;

        float skew = RandomRange(-50.0f, 50.0f);

        float rotationSpeed = RandomRange(2.0f, 2.0f + (4.0f * generation));

        foreach(ProcessUnit unit in children)
        {
            GameObject obj = Spawn(parent, unit);
            Transform tra = obj.transform;

            tra.localEulerAngles = new Vector3(0.0f, angle + phase, skew);
            tra.Translate(outwardRadius, 0.0f, 0.0f);
            float newScale = parent.localScale[0] * 0.96f;
            tra.localScale = new Vector3(newScale, newScale, newScale);

            if (unit.Content != '0')
            {
                obj.GetComponent<PlanetsBehaviour>().degreesPerSecond = rotationSpeed;
            }

                angle += deltaAngle;
            Grow(tra, unit.Children, generation + 1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit)
    {
        if(unit.Content == '0')
        {
            return Object.Instantiate(protoEmpty, parent);
        }

        GameObject obj =  Object.Instantiate(protoPlanet, parent);
        Material mat = materialLookup.Get(unit.Content);
        obj.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;

        return obj;
    }
}
