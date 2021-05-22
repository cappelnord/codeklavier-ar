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

        SpeciesVelocityMultiplier = 1.5f;

        base.Start();

        lsysController = LSystemController.Instance();
    }

    public override void Generate()
    {
        PreGenerate();

        Die();

        if (IsEmpty()) return;

        outer = Instantiate(OuterPrefab, transform);

        InteriaOuterIntensity ioi = outer.AddComponent<InteriaOuterIntensity>() as InteriaOuterIntensity;
        ioi.Gen = this;

        LifeBehaviour lbe = outer.AddComponent<LifeBehaviour>() as LifeBehaviour;
        lbe.TargetScale = new Vector3(1.1f, 1.1f, 1.1f);
        lbe.GrowStartTime = Time.time;

        FlowBehaviour fb = outer.AddComponent<FlowBehaviour>() as FlowBehaviour;
        fb.RotationMultiplier = 40f;
        fb.Gen = this;

        outer.GetComponent<RandomSpherePoints>().Gen = this;

        Grow(transform, lsys.Units[0], 0, 1.0f);
    }

    public override void Die(bool transformDeath = false)
    {
        if (outer != null)
        {
            Destroy(outer);
            outer = null;
        }
        base.Die(transformDeath);
    }

    public override float DeathVelocity(GameObject obj)
    {
        return 0.3f + 0.3f * SpeedMultiplier;
    }

    public override void SpawnBubble(GameObject prefab)
    {
        if (outer == null) return;

        float targetScale = RandomRange(0.01f, 0.06f);
        DoSpawnBubble(outer.transform, Random.insideUnitSphere.normalized * 0.51f, targetScale, prefab);
    }

    public override float SymbolDynamicsMultiplier(int dynamics, float contribution = 1.0f)
    {
        return 1.0f + CKARTools.LinLin(dynamics, 0, 127, -0.5f, 0.0f) * Config.SymbolDynamics * contribution;
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

            FlowBehaviour fbe = center.AddComponent<FlowBehaviour>() as FlowBehaviour;
            fbe.RotationMultiplier = 20f;
            fbe.Gen = this;

            InteriaInnerIntensity iii = center.AddComponent<InteriaInnerIntensity>() as InteriaInnerIntensity;
            iii.Gen = this;
            iii.KeyColor = materialLookup.GetColor('1');


            parent = center.transform;
        }

        int childrenIndex = -1;
        bool firstObject = true;

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
                obj.transform.localEulerAngles = LPrunastriGenerator.GetStartAngle(children.Count, childrenIndex);
            }
            else
            {
                obj.transform.localPosition = new Vector3(0.0f, 1.0f, 0.0f);
                obj.transform.localEulerAngles = new Vector3(0.0f, (float)childrenIndex / children.Count * 360.0f, 45.0f);
            }

            float myScale = thisScale * SymbolDynamicsMultiplier(unit.Dynamic, 0.5f);

            LifeBehaviour lbe = obj.AddComponent<LifeBehaviour>() as LifeBehaviour;
            lbe.TargetScale = new Vector3(myScale, myScale, myScale);
            lbe.GrowStartTime = Time.time + (0.5f * generation);

            FlowBehaviour fbe = obj.AddComponent<FlowBehaviour>() as FlowBehaviour;
            fbe.Gen = this;

            if(!firstObject)
            {
                // bit clumsy but topkek
                obj.GetComponent<MeshRenderer>().enabled = false;
            } else
            {
                InteriaInnerIntensity iii = obj.AddComponent<InteriaInnerIntensity>() as InteriaInnerIntensity;
                iii.Gen = this;
                iii.KeyColor = materialLookup.GetColor(unit.Content);
            }

            firstObject = false;
            Grow(obj.transform, unit.Children, generation + 1, thisScale);
        }
    }
}
