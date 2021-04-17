using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LInteriaGenerator : LGenerator
{

    public GameObject OuterPrefab;
    public GameObject InnerPrefab;

    private LSystemController lsysController;

    private GameObject outer;

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        lsysController = LSystemController.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        outer = Instantiate(OuterPrefab, transform);

        LifeBehaviour lbe = outer.AddComponent<LifeBehaviour>() as LifeBehaviour;
        lbe.TargetScale = new Vector3(1.0f, 1.0f, 1.0f);
        lbe.GrowStartTime = Time.time;

        Grow(transform, lsys.Units[0], 0, 1.0f);
    }

    public override void Die()
    {
        Destroy(outer);
        base.Die();
    }

    void Grow(Transform parent, List<ProcessUnit> children, int generation, float lastScale)
    {

        float thisScale = lastScale * 0.95f;

        if (generation == 0)
        {
            GameObject center = Instantiate(InnerPrefab, parent);

            LifeBehaviour lbe = center.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lbe.TargetScale = new Vector3(0.15f, 0.15f, 0.15f);
            lbe.GrowStartTime = Time.time + (0.5f * generation);

            parent = center.transform;
        }

        int childrenIndex = -1;
        foreach (ProcessUnit unit in children)
        {
            childrenIndex++;

            if (!VisitUnit(unit) || unit.Content == '0')
            {
                continue;
            }

            GameObject obj = Instantiate(InnerPrefab, parent);

            if (generation == 0)
            {
                obj.transform.localEulerAngles = LPrunastriGenerator.StartAngles[children.Count - 1][childrenIndex];
            }
            else
            {
                obj.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
                obj.transform.localEulerAngles = new Vector3(0.0f, (float)childrenIndex / children.Count * 360.0f, 45.0f);
            }

            LifeBehaviour lbe = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lbe.TargetScale = new Vector3(thisScale, thisScale, thisScale);
            lbe.GrowStartTime = Time.time + (0.5f * generation);

            Grow(obj.transform, unit.Children, generation + 1, thisScale);
        }
    }
}
