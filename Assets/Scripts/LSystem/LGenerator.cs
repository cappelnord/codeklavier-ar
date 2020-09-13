using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGenerator : LSystemBehaviour
{

    public TransformSpec TransformSpec;
    public Bounds Bounds;
    public float LastTimeTouched;
    public System.Random Rand;
    public float SpeedMultiplier = 1.0f;

    protected float scaleMultiplier;
    protected float positionMultiplier = 1.0f;
    protected MaterialLookup materialLookup;
    protected Dictionary<long, ProcessUnit> visitedUnitDict;

    private IIRFilter velocityValueFilter = new IIRFilter(0.5f, 0.01f);
    private string velValueKey;
    private IIRFilter speedValueFilter = new IIRFilter(0.5f, 0.01f);
    private string speedValueKey;
    private IIRFilter floatUp = new IIRFilter(0.0f, 0.002f);

    protected virtual void Awake()
    {

    }

    protected virtual void Start()
    {
        materialLookup = MaterialLookup.Instance();
        Touch();
        SparseUpdate();
    }

    void Touch() => LastTimeTouched = Time.time;

    void OnEnable() => EventManager.OnValue += OnValue;

    void OnDisable() => EventManager.OnValue -= OnValue;

    void OnValue(string key, float value)
    {
        if (lsys == null) return;

        Touch();

        if(key == velValueKey)
        {
            velocityValueFilter.Set(value);
        }

        if (key == speedValueKey)
        {
            speedValueFilter.Set(value);
        }
    }

    protected void PreGenerate()
    {
        ResetRandomness();
        Touch();
        visitedUnitDict = new Dictionary<long, ProcessUnit>();

    }

    protected bool VisitUnit(ProcessUnit unit)
    {
        if(!visitedUnitDict.ContainsKey(unit.Id))
        {
            visitedUnitDict[unit.Id] = unit;
            return true;
        } else
        {
            return false;
        }
    }

    Bounds GetBounds()
    {
        Bounds combinedBounds = new Bounds(transform.position, new Vector3(0.0f, 0.0f, 0.0f));
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer renderer in renderers)
        {
             combinedBounds.Encapsulate(renderer.bounds);
        }

        return combinedBounds;
    }

    public void SparseUpdate()
    {
        // GeneratorHerd should clean up
        if (gameObject == null) return;

        Bounds = GetBounds();

        if (Config.FloatUp)
        {
            Vector3 size = Bounds.size;
            Vector3 center = Bounds.center;
            float bottom;
            if (Config.WorldIsAR)
            {
                // See if it's right!
                bottom = center[2] - size[2] / 2;
            }
            else
            {
                bottom = center[1] - size[1] / 2;
            }

            if (bottom > 0.0f)
            {
                bottom = 0.0f;
            }

            floatUp.Set(-bottom);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldAct())
        {
            Generate();
        }

        TransformSpec = lsys.TransformSpec;

        scaleMultiplier = 0.25f + (velocityValueFilter.Filter() * 1.75f);
        SpeedMultiplier = 1.0f / speedValueFilter.Filter() * 0.25f;

        ApplyTransformSpec();
    }

    public float RandomRange(float min, float max)
    {
        return (float) (min + (Rand.NextDouble() * (max - min)));
    }

    public int RandomRange(int min, int max)
    {
        return Rand.Next(min, max);
    }

    public void ResetRandomness()
    {
        char[] chars = lsys.RulesString.ToCharArray();
        int sum = 0;
        for(int i = 0; i < chars.Length; i++)
        {
            sum = sum + chars[i];
        }

        Rand = new System.Random(sum % 100000);
    }

    virtual public void Generate()
    {

    }

    virtual public void ApplyTransformSpec()
    {
        gameObject.transform.localPosition = new Vector3(TransformSpec.Position[0] * positionMultiplier, TransformSpec.Position[1] * positionMultiplier + floatUp.Filter(), TransformSpec.Position[2] * positionMultiplier);
        gameObject.transform.localScale = new Vector3(TransformSpec.Scale[0] * scaleMultiplier, TransformSpec.Scale[1] * scaleMultiplier, TransformSpec.Scale[2] * scaleMultiplier);
        gameObject.transform.localEulerAngles = new Vector3(TransformSpec.Rotation[0], TransformSpec.Rotation[1], TransformSpec.Rotation[2]);
    }

    override public void OnLSystemSet()
    {
        velValueKey = $"{lsys.Key}-vel";
        velocityValueFilter.Init(ValueStore.Get(velValueKey, 0.5f));

        speedValueKey = $"{lsys.Key}-speed";
        speedValueFilter.Init(ValueStore.Get(velValueKey, 0.5f));

        base.OnLSystemSet();
    }
}
