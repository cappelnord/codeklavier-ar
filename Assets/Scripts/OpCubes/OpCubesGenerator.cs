using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpCubesGenerator : LGenerator
{

    public GameObject protoCube;
    public GameObject protoEmpty;

    protected override void Start()
    {
        base.Start();
    }

    override public void Generate()
    {
        PreGenerate();

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child == transform) continue;

            OpCubeBehaviour behave = child.gameObject.GetComponent<OpCubeBehaviour>();
            if (behave != null)
            {
                behave.Die();
            } 

            // TODO: REFACTOR SO THAT ALSO EMPTY OBJECTS GET CLEANED UP ALSO IN OTHER PLACES
        }

        Grow(gameObject.transform, lsys.units[0], 0.7f, 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, float lastScale, int generation)
    {
        float outwardRadius = 2.0f - (generation * RandomRange(0.02f, 0.08f));

        // add an empty in center if not-singular axiom
        if (generation == 0 && children.Count == 1)
        {
            outwardRadius = 0.0f;
        } else
        
        if (generation == 0 && children.Count > 1) {
            GameObject empty = Instantiate(protoEmpty, parent);
            parent = empty.transform;
        }

        float phase = RandomRange(0.0f, 360.0f);
        float angle = 0.0f;
        float deltaAngle = 360.0f / children.Count;

        float skew = RandomRange(-50.0f, 50.0f);

        float rotationSpeed = RandomRange(2.0f, 2.0f + (4.0f * generation));

        foreach (ProcessUnit unit in children)
        {
            GameObject obj;
            float newScale = lastScale * 0.99f;
            bool constructObject = unit.Content != '0' && VisitUnit(unit);
            if (constructObject)
            {
                obj = Spawn(parent, unit, generation);
                obj.GetComponent<OpCubeBehaviour>().targetScale = newScale;
                obj.GetComponent<OpCubeBehaviour>().degreesPerSecond = RandomRange(-3.0f, 3.0f);
                obj.GetComponent<OpCubeBehaviour>().gen = this;
            } else
            {
                obj = Instantiate(protoEmpty, parent);
            }

            Transform tra = obj.transform;
            tra.localEulerAngles = new Vector3(0.0f, angle + phase, skew);
            tra.localPosition = new Vector3(outwardRadius, 0.0f, 0.0f) + new Vector3(RandomRange(-0.2f, 0.2f), RandomRange(-0.2f, 0.2f), RandomRange(-0.2f, 0.2f));

            angle += deltaAngle;
            Grow(tra, unit.Children, newScale, generation + 1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit, int generation)
    {
        if (unit.Content == '0')
        {
            return Object.Instantiate(protoEmpty, parent);
        }

        
        GameObject obj = Object.Instantiate(protoCube, parent);
        SineStripesCycler ssc = obj.GetComponent<SineStripesCycler>();
        ssc.phaseFrequencyX = RandomRange(0.2f, 8.0f - (generation * 0.5f));
        ssc.phaseFrequencyY = RandomRange(0.2f, generation * 0.05f);

        float xyAmb = 4.0f - (generation * 0.3333f);

        ssc.phaseOffsetX = Random.Range(-xyAmb, xyAmb);
        ssc.phaseOffsetY = Random.Range(-xyAmb, xyAmb);

        if(generation % 2 == 1)
        {
            ssc.color = Color.black;
            ssc.backgroundColor = materialLookup.GetColor(unit.Content);
        } else
        {
            ssc.color = Color.white;
            ssc.backgroundColor = Color.black;

        }

        return obj;
    }
}
