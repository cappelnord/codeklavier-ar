using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class RingSegment
{
    public int NumSegments;
    public float StartRotation;
    public ProcessUnit Unit;

    public RingSegment(ProcessUnit unit, int numSegments, float startRotation)
    {
        Unit = unit;
        NumSegments = numSegments;
        StartRotation = startRotation;
    }
}

public class LArcBallGenerator : LGenerator {

    public Material[] Materials = new Material[10];
    public GameObject RingProto;

    private Dictionary<char, Material> lookup = new Dictionary<char, Material>();
    private LSystemController lsysController;
    private ArcMeshGen meshgen;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        lsysController = LSystemController.Instance();
        meshgen = ArcMeshGen.Instance();

        int i = 0;
        foreach (char symbol in lsysController.symbols)
        {
            lookup.Add(symbol, Materials[i]);
            i++;
        }
    }

    public override void Generate()
    {
        PreGenerate();

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<RingBehaviour>().Die();
        }

        float scale = 0.05f;

        foreach(List<ProcessUnit> data in lsys.units)
        {
            if (data.Count == 0) break;

            float dynamicsSum = 0.0f;
            foreach(ProcessUnit unit in data)
            {
                dynamicsSum += unit.Dynamic;
            }

            dynamicsSum = dynamicsSum / data.Count;
            float speedMul = (0.05f + (2.45f / 128.0f * dynamicsSum)) * 60.0f * 0.2f;
            float comp = 1.0f / scale;
            float rotateCompensation = 0.1f / scale;

            GameObject ring = GameObject.Instantiate(RingProto, transform);
            ring.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);

            ring.transform.Rotate(new Vector3(RandomRange(0.0f, 360.0f), RandomRange(0.0f, 360.0f), RandomRange(0.0f, 360.0f)));

            RingBehaviour ringBehave = ring.GetComponent<RingBehaviour>();
            ringBehave.Rotation = new Vector3(RandomRange(-2.0f, 2.0f) * rotateCompensation * speedMul, RandomRange(-2.0f, 2.0f) * rotateCompensation * speedMul, RandomRange(-2.0f, 2.0f) * rotateCompensation * speedMul);
            ringBehave.TargetScale = scale;
            ringBehave.Scale = 0.0f;
            ringBehave.gen = this;

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
                    if (lastSegment.Unit.Content != unit.Content)
                    {
                        lastSegment = new RingSegment(unit, 0, startRotation);
                        list.Add(lastSegment);
                    }
                }

                lastSegment.NumSegments = lastSegment.NumSegments + 1;
                startRotation += degsPerSegment;
            }


            // fehler tritt bei doppelten auf

            startRotation = 0.0f;
            foreach(RingSegment arc in list)
            {
                startRotation += (arc.NumSegments * degsPerSegment);

                if (arc.Unit.Content != '0')
                {
                    float width = (0.2f + (2.0f / 128.0f * arc.Unit.Dynamic)) * 0.1f;
                    GameObject obj = meshgen.GetArcObject(((arc.NumSegments * degsPerSegment)) * Mathf.Deg2Rad, 0.4f * comp * 0.1f, width * comp, ring.transform, lookup[arc.Unit.Content]);
                    obj.transform.Rotate(new Vector3(0.0f, 0.0f, startRotation), Space.Self);
                }
            }

            scale += 0.1f;
        }
    }
}
