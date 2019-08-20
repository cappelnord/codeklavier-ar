using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpCubesGenerator : LGenerator
{

    public GameObject protoCube;
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
            OpCubeBehaviour behave = child.gameObject.GetComponent<OpCubeBehaviour>();
            if (behave != null)
            {
                behave.Die();
            }
        }

        Grow(gameObject.transform, lsys.units[0], 0.7f, 0);
    }

    void Grow(Transform parent, List<ProcessUnit> children, float lastScale, int generation)
    {
        float outwardRadius = 1.5f - (generation * RandomRange(0.02f, 0.08f));
        // add an empty in center if not-singular axiom
        if (generation == 0 && children.Count == 1)
        {
            outwardRadius = 0.0f;
        }

        float phase = RandomRange(0.0f, 360.0f);
        float angle = 0.0f;
        float deltaAngle = 360.0f / children.Count;

        float skew = RandomRange(-50.0f, 50.0f);

        float rotationSpeed = RandomRange(2.0f, 2.0f + (4.0f * generation));

        foreach (ProcessUnit unit in children)
        {
            GameObject obj = Spawn(parent, unit);
            Transform tra = obj.transform;

            float newScale = lastScale * 0.99f;

            tra.localEulerAngles = new Vector3(0.0f, angle + phase, skew);
            tra.localPosition = new Vector3(outwardRadius, 0.0f, 0.0f);

            if(unit.Content != '0')
            {
                obj.GetComponent<OpCubeBehaviour>().targetScale = newScale;
                obj.GetComponent<OpCubeBehaviour>().degreesPerSecond = RandomRange(-3.0f, 3.0f);
            }

            angle += deltaAngle;
            Grow(tra, unit.Children, newScale, generation + 1);
        }
    }

    GameObject Spawn(Transform parent, ProcessUnit unit)
    {
        if (unit.Content == '0')
        {
            return Object.Instantiate(protoEmpty, parent);
        }

        
        GameObject obj = Object.Instantiate(protoCube, parent);
        SineStripesCycler ssc = obj.GetComponent<SineStripesCycler>();
        ssc.phaseFrequencyX = RandomRange(0.2f, 8.0f);
        ssc.phaseFrequencyY = 0.0f;
        ssc.phaseOffsetX = Random.Range(-5.0f, 5.0f);
        ssc.phaseOffsetY = Random.Range(-5.0f, 5.0f);

        return obj;
    }
}
