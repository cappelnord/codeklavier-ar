using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpCubesGenerator : LGenerator
{

    public GameObject ProtoCube;
    public GameObject ProtoEmpty;

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

            OpCubeBehaviour behave = GetComponent<OpCubeBehaviour>();
            if(behave != null)
            {
                behave.Die();
            } else
            {
                Destroy(child.gameObject);
            }
        }

        Grow(gameObject.transform, lsys.Units[0], 0.7f, 0);
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
            GameObject empty = Instantiate(ProtoEmpty, parent);
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
                obj.GetComponent<OpCubeBehaviour>().TargetScale = newScale;
                obj.GetComponent<OpCubeBehaviour>().DegreesPerSecond = RandomRange(-3.0f, 3.0f);
                obj.GetComponent<OpCubeBehaviour>().Gen = this;
            } else
            {
                obj = Instantiate(ProtoEmpty, parent);
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
            return Object.Instantiate(ProtoEmpty, parent);
        }

        
        GameObject obj = Object.Instantiate(ProtoCube, parent);
        SineStripesCycler ssc = obj.GetComponent<SineStripesCycler>();
        ssc.PhaseFrequencyX = RandomRange(0.2f, 8.0f - (generation * 0.5f));
        ssc.PhaseFrequencyY = RandomRange(0.2f, generation * 0.05f);

        float xyAmb = 4.0f - (generation * 0.3333f);

        ssc.PhaseOffsetX = Random.Range(-xyAmb, xyAmb);
        ssc.PhaseOffsetY = Random.Range(-xyAmb, xyAmb);

        if(generation % 2 == 1)
        {
            ssc.Color = Color.black;
            ssc.BackgroundColor = materialLookup.GetColor(unit.Content);
        } else
        {
            ssc.Color = Color.white;
            ssc.BackgroundColor = Color.black;

        }

        return obj;
    }
}
