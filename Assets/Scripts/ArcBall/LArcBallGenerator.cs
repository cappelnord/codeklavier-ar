using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RingSegment
{
    public int numSegments;
    public float startRotation;
    public ProcessUnit unit;

    public RingSegment(ProcessUnit _unit, int _numSegments, float _startRotation)
    {
        unit = _unit;
        numSegments = _numSegments;
        startRotation = _startRotation;
    }
}

public class LArcBallGenerator : LGenerator {

    public Material[] materials = new Material[10];
    private Dictionary<char, Material> lookup;

    public GameObject ringProto;

    private LSystemController lsysController;

    private ArcMeshGen meshgen;

    // Start is called before the first frame update
    void Start()
    {
        lsysController = LSystemController.Instance();
        meshgen = ArcMeshGen.Instance();

        lookup = new Dictionary<char, Material>();

        int i = 0;
        foreach (char symbol in lsysController.symbols)
        {
            lookup.Add(symbol, materials[i]);
            i++;
        }
    }

    override public void Generate()
    {
        ResetRandomness();

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<RingBehaviour>().Die();
        }

        float scale = 0.5f;

        foreach(List<ProcessUnit> data in lsys.units)
        {
            if (data.Count == 0) break;

            float dynamicsSum = 0.0f;
            foreach(ProcessUnit unit in data)
            {
                dynamicsSum += unit.Dynamic;
            }

            dynamicsSum = dynamicsSum / data.Count;
            float speedMul = 0.05f + (2.45f / 128.0f * dynamicsSum) * 60.0f;
            float comp = 1.0f / scale;

            GameObject ring = GameObject.Instantiate(ringProto, transform);
            ring.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

            ring.transform.Rotate(new Vector3(RandomRange(0.0f, 360.0f), RandomRange(0.0f, 360.0f), RandomRange(0.0f, 360.0f)));
            ring.GetComponent<RingBehaviour>().rotation = new Vector3(RandomRange(-2.0f, 2.0f) * comp * speedMul, RandomRange(-2.0f, 2.0f) * comp * speedMul, RandomRange(-2.0f, 2.0f) * comp * speedMul);
            ring.GetComponent<RingBehaviour>().targetScale = scale;
            ring.GetComponent<RingBehaviour>().scale = 0.0f;

            float degsPerSegment = 360.0f / data.Count;
            float startRotation = 0.0f;

            List<RingSegment> list = new List<RingSegment>();
            RingSegment lastSegment = null;

            // clump segments
            // TODO: Join segments
            foreach(ProcessUnit unit in data)
            {
                if (lastSegment == null)
                {
                    lastSegment = new RingSegment(unit, 0, startRotation);
                    list.Add(lastSegment);
                }
                else
                {
                    if (lastSegment.unit.Content != unit.Content)
                    {
                        lastSegment = new RingSegment(unit, 0, startRotation);
                        list.Add(lastSegment);
                    }
                }

                lastSegment.numSegments = lastSegment.numSegments + 1;
                startRotation += degsPerSegment;
            }


            // fehler tritt bei doppelten auf

            startRotation = 0.0f;
            foreach(RingSegment arc in list)
            {
                startRotation += (arc.numSegments * degsPerSegment);

                if (arc.unit.Content != '0')
                {
                    float width = 0.2f + (2.0f / 128.0f * arc.unit.Dynamic);
                    GameObject obj = meshgen.GetArcObject(((arc.numSegments * degsPerSegment)) * Mathf.Deg2Rad, 0.4f * comp, width * comp, ring.transform, lookup[arc.unit.Content]);
                    obj.transform.Rotate(new Vector3(0.0f, 0.0f, startRotation), Space.Self);
                }
            }

            scale += 1.0f;
        }
    }
}
