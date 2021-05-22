using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGenerator : LSystemBehaviour
{

    public bool Variety = false;

    [HideInInspector]
    public TransformSpec TransformSpec;

    [HideInInspector]
    public Bounds Bounds;

    [HideInInspector]
    public float LastTimeTouched;

    [HideInInspector]
    public System.Random Rand;

    [HideInInspector]
    public float SpeedMultiplier = 1.0f;

    [HideInInspector]
    public float Intensity = 0.0f;

    [HideInInspector]
    public float ColorIntensity = 0.0f;

    [HideInInspector]
    public GeneratorHerd Herd;

    [HideInInspector]
    public Vector3 Position;

    public float ScaleMultiplier;
    protected float positionMultiplier = 1.0f;
    protected MaterialLookup materialLookup;
    protected Dictionary<long, ProcessUnit> visitedUnitDict;

    private IIRFilter velocityValueFilter = new IIRFilter(0.5f, 0.01f);
    private string velValueKey;
    private IIRFilter speedValueFilter = new IIRFilter(0.5f, 0.003f);
    private string intensityValueKey;
    private IIRFilter colorIntensityValueFilter = new IIRFilter(0.0f, 0.01f);
    private string colorIntensityValueKey;
    private IIRFilter intensityValueFilter = new IIRFilter(0.0f, 0.01f);
    private string speedValueKey;
    private IIRFilter floatUp = new IIRFilter(0.0f, 0.002f);

    private TransformSpec lastTransformSpec;
    private bool generated = true;


    protected float SpeciesVelocityMultiplier = 1.0f;
    protected float SpeciesSpeedMultiplier = 1.0f;

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

        if (key == velValueKey)
        {
            velocityValueFilter.Set(value);
        }

        if (key == speedValueKey)
        {
            speedValueFilter.Set(value);
        }

        if (key == intensityValueKey)
        {
            intensityValueFilter.Set(value);
        }

        if (key == colorIntensityValueKey)
        {
            colorIntensityValueFilter.Set(value);
        }
    }

    protected void PreGenerate()
    {
        generated = true;
        ResetRandomness();
        Touch();
        visitedUnitDict = new Dictionary<long, ProcessUnit>();

    }

    protected bool VisitUnit(ProcessUnit unit)
    {
        if (!visitedUnitDict.ContainsKey(unit.Id))
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
    protected virtual void Update()
    {
        if (ShouldAct())
        {
            Generate();
            generated = true;
        }

        TransformSpec = lsys.TransformSpec;

        if(lastTransformSpec != TransformSpec && generated && lastTransformSpec != null)
        {
            Die(true);
            Generate();
        }

        lastTransformSpec = TransformSpec;

        ScaleMultiplier = 0.25f + (velocityValueFilter.Filter() * 1.75f * SpeciesVelocityMultiplier);
        SpeedMultiplier = 0.3f + ((1.0f / speedValueFilter.Filter()) * 0.15f * SpeciesSpeedMultiplier);
        Intensity = intensityValueFilter.Filter();
        ColorIntensity = colorIntensityValueFilter.Filter();

        ApplyTransformSpec();
    }

    virtual public void Die(bool transformDeath = false)
    {
        if (Bounds == null)
        {
            Bounds = GetBounds();
        }

        List<Transform> transforms = new List<Transform>();
        foreach (Transform child in transform)
        {
            transforms.Add(child);
        }

        if (Herd.SimpleDeath || WorldIsBoxed.Status)
        {
            foreach (Transform child in transforms)
            {
                Destroy(child.gameObject);
            }
            return;
        }

        foreach (Transform child in transforms)
        {
            DieIterate(child, transformDeath);
        }


        // this fixes an issue that sometimes BubbleBehaviours are disabled in the trash .. 
        // .. idk how this happens! This should at least take care of a build up of bubbles.

        foreach(BubbleBehaviour bb in Herd.Trash.GetComponentsInChildren<BubbleBehaviour>())
        {
            if(!bb.enabled)
            {
                bb.enabled = true;
            }
        }
    }

    public virtual void DieIterate(Transform t, bool transformDeath)
    {
        List<Transform> transforms = new List<Transform>();
        foreach (Transform child in t)
        {
            transforms.Add(child);
        }

        foreach (Transform child in transforms)
        {
            DieIterate(child, transformDeath);
        }

        MoveObjectToTrash(t.gameObject, transformDeath);
    }

    virtual public void MoveObjectToTrash(GameObject obj, bool transformDeath)
    {
        BubbleBehaviour bb = obj.GetComponent<BubbleBehaviour>();
        if (bb)
        {
            bb.Dislodge();
        } else
        {
            obj.transform.SetParent(Herd.Trash);
            Destroy(obj.GetComponent<LifeBehaviour>());
            Destroy(obj.GetComponent<FlowBehaviour>());
            Destroy(obj.GetComponent<IntensityBehaviour>());

            if (!transformDeath)
            {
                DeathBehaviour db = obj.AddComponent<DeathBehaviour>() as DeathBehaviour;
                db.Velocity = DeathVelocity(obj);
                db.Rotation = new Vector3(RandomRange(-20.0f, 20.0f), RandomRange(-20.0f, 20.0f), RandomRange(-20.0f, 20.0f));
                db.Direction = Vector3.Normalize(obj.transform.position - Bounds.center) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                db.ShrinkStartTime = Time.time + RandomRange(2f, 8f);
                db.ScaleRelativeToWorld = Herd.Trash.lossyScale.x;
            } else
            {
                TransformDeathBehaviour db = obj.AddComponent<TransformDeathBehaviour>() as TransformDeathBehaviour;
                db.Velocity = DeathVelocity(obj) * 0.5f;
                db.Rotation = new Vector3(RandomRange(-40.0f, 40.0f), RandomRange(-40.0f, 40.0f), RandomRange(-40.0f, 40.0f));
                db.Direction = Vector3.Normalize(obj.transform.position - Bounds.center) + new Vector3(Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f), Random.Range(-0.1f, 0.1f));
                db.ShrinkStartTime = Time.time;
                db.ScaleRelativeToWorld = Herd.Trash.lossyScale.x;
            }
        }

    }

    virtual public float DeathVelocity(GameObject obj)
    {
        return 0.1f + 0.3f * SpeedMultiplier;
    }

    public float RandomRange(float min, float max)
    {
        if (Rand == null) return min;
        return (float)(min + (Rand.NextDouble() * (max - min)));
    }

    public int RandomRange(int min, int max)
    {
        if (Rand == null) return min;
        return Rand.Next(min, max);
    }

    public void ResetRandomness()
    {
        char[] chars;
        if (lsys.RulesString != null)
        {
            chars = lsys.RulesString.ToCharArray();
        } else
        {
            chars = "OK-Random!".ToCharArray();
        }

        int sum = 0;

        for (int i = 0; i < chars.Length; i++)
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
        Position = new Vector3(TransformSpec.Position[0] * positionMultiplier, TransformSpec.Position[1] * positionMultiplier + floatUp.Filter(), TransformSpec.Position[2] * positionMultiplier);
        gameObject.transform.localPosition = Position;
        gameObject.transform.localScale = new Vector3(TransformSpec.Scale[0] * ScaleMultiplier, TransformSpec.Scale[1] * ScaleMultiplier, TransformSpec.Scale[2] * ScaleMultiplier);
        gameObject.transform.localEulerAngles = new Vector3(TransformSpec.Rotation[0], TransformSpec.Rotation[1], TransformSpec.Rotation[2]);
    }

    virtual public void SpawnBubble(GameObject prefab)
    {
        List<Transform> transforms = new List<Transform>(GetComponentsInChildren<Transform>());
        Transform target = transforms[Random.Range(0, transforms.Count)];

        MeshFilter mf = target.gameObject.GetComponent<MeshFilter>();

        if (mf)
        {
            Mesh mesh = mf.mesh;

            float targetScale = RandomRange(0.01f, 0.06f);

            Vector3[] vertices = mesh.vertices;
            Vector3 randomVertex = vertices[Random.Range(0, vertices.Length)] * (1f + targetScale);

            DoSpawnBubble(target, randomVertex, targetScale, prefab);
        }
    }

    public void DoSpawnBubble(Transform target, Vector3 position, float targetScale, GameObject prefab)
    {
        GameObject bubble = Instantiate(prefab, target);

        bubble.transform.localPosition = position;

        BubbleBehaviour bb = bubble.GetComponent<BubbleBehaviour>();

        bb.TargetTime = Time.time + RandomRange(2f, 4f);
        bb.TargetScale = targetScale;
        bb.TargetTransform = Herd.Trash;
        bb.Gen = this;
    }

    override public void OnLSystemSet()
    {
        velValueKey = $"{lsys.Key}-vel";
        velocityValueFilter.Init(ValueStore.Get(velValueKey, 0.5f));

        speedValueKey = $"{lsys.Key}-speed";
        speedValueFilter.Init(ValueStore.Get(speedValueKey, 0.5f));

        intensityValueKey = $"{lsys.Key}-intensity";
        intensityValueFilter.Init(ValueStore.Get(intensityValueKey, 0.0f));

        colorIntensityValueKey = $"{lsys.Key}-color";
        colorIntensityValueFilter.Init(ValueStore.Get(colorIntensityValueKey, 0.0f));

        base.OnLSystemSet();
    }

    public bool IsEmpty()
    {
        return lsys.Units[0].Count == 1 && lsys.Units[0][0].Content == '0';
    }
}
