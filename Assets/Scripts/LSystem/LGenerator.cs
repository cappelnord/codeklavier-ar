using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGenerator : LSystemBehaviour
{

    public TransformSpec transformSpec;

    public System.Random rand;

    private IIRFilter velocityValueFilter;
    private string velValueKey;

    private IIRFilter speedValueFilter;
    private string speedValueKey;

    public float speedMultiplier = 1.0f;

    protected float scaleMultiplier;
    protected float positionMultiplier = 1.0f;

    protected MaterialLookup materialLookup;

    protected Dictionary<long, ProcessUnit> visitedUnitDict;

    private IIRFilter floatUp;

    protected virtual void Awake()
    {
        velocityValueFilter = new IIRFilter(0.5f, 0.01f);
        speedValueFilter = new IIRFilter(0.5f, 0.01f);

        floatUp = new IIRFilter(0.0f, 0.002f);
    }

    protected virtual void Start()
    {
        materialLookup = MaterialLookup.Instance();
    }


    void OnEnable()
    {
        EventManager.OnValue += OnValue;
    }

    void OnDisable()
    {
        EventManager.OnValue -= OnValue;
    }

    void OnValue(string key, float value)
    {
        if (lsys == null) return;

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
        visitedUnitDict = new Dictionary<long, ProcessUnit>();

    }

    protected bool VisitUnit(ProcessUnit unit)
    {
        if(!visitedUnitDict.ContainsKey(unit.id))
        {
            visitedUnitDict[unit.id] = unit;
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

    // don't calc on every frame; find some solution!
    void SparseUpdate()
    {
        Bounds bounds = GetBounds();
        Vector3 size = bounds.size;
        Vector3 center = bounds.center;
        float bottom;
        if(WorldIsAR.Get()) {
            // See if it's right!
            bottom = center[2] - size[2] / 2;
        }
        else
        {
            bottom = center[1] - size[1] / 2;
        }

       if(bottom > 0.0f)
        {
            bottom = 0.0f;
        }

        floatUp.Set(-bottom);

    }

    // Update is called once per frame
    void Update()
    {
        SparseUpdate();

        if (ShouldAct())
        {
            Generate();
        }

        if (lsys.transformSpec != transformSpec) {
            transformSpec = lsys.transformSpec;
        }

        scaleMultiplier = 0.25f + (velocityValueFilter.Filter() * 1.75f);
        speedMultiplier = 1.0f / speedValueFilter.Filter() * 0.25f;

        ApplyTransformSpec();
    }

    public float RandomRange(float min, float max)
    {
        return (float) (min + (rand.NextDouble() * (max - min)));
    }

    public int RandomRange(int min, int max)
    {
        return rand.Next(min, max);
    }

    public void ResetRandomness()
    {
        char[] chars = lsys.rulesString.ToCharArray();
        int sum = 0;
        for(int i = 0; i < chars.Length; i++)
        {
            sum = sum + chars[i];
        }

        rand = new System.Random(sum % 100000);
    }

    virtual public void Generate()
    {

    }

    virtual public void ApplyTransformSpec()
    {
        gameObject.transform.localPosition = new Vector3(transformSpec.position[0] * positionMultiplier, transformSpec.position[1] * positionMultiplier + floatUp.Filter(), transformSpec.position[2] * positionMultiplier);
        gameObject.transform.localScale = new Vector3(transformSpec.scale[0] * scaleMultiplier, transformSpec.scale[1] * scaleMultiplier, transformSpec.scale[2] * scaleMultiplier);
        gameObject.transform.localEulerAngles = new Vector3(transformSpec.rotation[0], transformSpec.rotation[1], transformSpec.rotation[2]);
    }

    override public void OnLSystemSet()
    {
        velValueKey = lsys.key + "-vel";
        velocityValueFilter.Init(ValueStore.Get(velValueKey, 0.5f));

        speedValueKey = lsys.key + "-speed";
        speedValueFilter.Init(ValueStore.Get(velValueKey, 0.5f));

        base.OnLSystemSet();
    }
}
