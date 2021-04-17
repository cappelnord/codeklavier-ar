using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorObject
{
    public GameObject Obj;
    public string Shape;
    public LGenerator LGen;

    public GeneratorObject(GameObject obj, string shape)
    {
        Obj = obj;
        Shape = shape;
        LGen = obj.GetComponent<LGenerator>();
    }
}

public class GeneratorHerd : MonoBehaviour
{
    private LSystemController LSysController;
    private Dictionary<string, GeneratorObject> Objects = new Dictionary<string, GeneratorObject>();
    private Dictionary<string, GameObject> shapeLookup;

    public GameObject Shape1;
    public GameObject Shape2;
    public GameObject Shape3;
    public GameObject Shape4;
    public GameObject Shape5;
    public GameObject Shape6;
    public GameObject Shape7;

    private int sparseUpdateCounter = 0;

    [HideInInspector]
    public Transform Trash;

    public bool SimpleDeath = false;

    // Start is called before the first frame update
    void Start()
    {
        LSysController = LSystemController.Instance();
        shapeLookup = new Dictionary<string, GameObject>()
        {
            {"1", Shape1 },
            {"2", Shape2 },
            {"3", Shape3 },
            {"4", Shape4 },
            {"5", Shape5 },
            {"6", Shape6 },
            {"7", Shape7 }
        };

        Trash = transform.Find("Trash").transform;
    }

    // Update is called once per frame
    void Update()
    {
        foreach(string key in LSysController.Forest.Keys)
        {
            // TODO: Bit weird logic, maybe use 2 distinct variables
            bool create = !Objects.ContainsKey(key);
            if(!create)
            {
                create = Objects[key].Shape != LSysController.Forest[key].Shape;
                if(create)
                {
                    Objects[key].Obj.GetComponent<LGenerator>().Die();
                    Destroy(Objects[key].Obj);
                }
            }

            if(create)
            {
                Objects[key] = Create(LSysController.Forest[key]);
            }
        }

        // do sparse updates
        List<GeneratorObject> objectsList = new List<GeneratorObject>(Objects.Values);
        if (objectsList.Count > 0)
        {
            if (Time.frameCount % 2 == 0)
            {
                int idx = sparseUpdateCounter % objectsList.Count;
                objectsList[idx].LGen.SparseUpdate();
                sparseUpdateCounter++;
            }
        }
    }

    GeneratorObject Create(LSystem lsys)
    {
        string shape = lsys.Shape;

        GameObject obj = Instantiate(shapeLookup[shape], transform);
        obj.GetComponent<LGenerator>().SetLSystem(lsys);
        obj.GetComponent<LGenerator>().SetHerd(this);


        return new GeneratorObject(obj, shape);
    }

    public Bounds GetBounds()
    {
        Bounds combinedBounds = new Bounds(transform.position, new Vector3(0.0f, 0.0f, 0.0f));
        foreach(GeneratorObject obj in Objects.Values)
        {
            combinedBounds.Encapsulate(obj.LGen.Bounds);
        }
        return combinedBounds;
    }

    public Vector3 CenterOfActivity()
    {
        float timeGate = 20.0f;

        float sumWeights = 0.0f;
        Vector3 sumPosition = new Vector3(0.0f, 0.0f, 0.0f);

        float ct = Time.time;

        foreach(GeneratorObject obj in Objects.Values)
        {
            LGenerator lgen = obj.LGen;
            float dt = ct - lgen.LastTimeTouched;
            if(dt <= timeGate)
            {
                float weight = timeGate - dt;
                sumWeights += weight;
                sumPosition = sumPosition + (lgen.Bounds.center * weight);
            }
        }

        if(sumWeights <= 5.0f)
        {
            sumWeights = 5.0f;
        }

        return sumPosition / sumWeights;
    }
}
