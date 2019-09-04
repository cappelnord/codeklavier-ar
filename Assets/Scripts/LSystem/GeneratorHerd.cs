using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorObject
{
    public GameObject obj;
    public string shape;
    public LGenerator lgen;

    public GeneratorObject(GameObject _obj, string _shape)
    {
        obj = _obj;
        shape = _shape;
        lgen = _obj.GetComponent<LGenerator>();
    }
}

public class GeneratorHerd : MonoBehaviour
{
    private LSystemController lsysController;
    private Dictionary<string, GeneratorObject> objects;

    public GameObject shape1;
    public GameObject shape2;
    public GameObject shape3;
    public GameObject shape4;
    public GameObject shape5;
    public GameObject shape6;
    public GameObject shape7;

    private int sparseUpdateCounter = 0;

    // Start is called before the first frame update
    void Start()
    {
        objects = new Dictionary<string, GeneratorObject>();
        lsysController = LSystemController.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(string key in lsysController.forrest.Keys)
        {
            bool create = !objects.ContainsKey(key);
            if(!create)
            {
                create = objects[key].shape != lsysController.forrest[key].shape;
                if(create)
                {
                    Destroy(objects[key].obj);
                }
            }

            if(create)
            {
                objects[key] = Create(lsysController.forrest[key]);
            }
        }

        // do sparse updates
        List<GeneratorObject> objectsList = new List<GeneratorObject>(objects.Values);
        if (objectsList.Count > 0)
        {
            if (Time.frameCount % 2 == 0)
            {
                int idx = sparseUpdateCounter % objectsList.Count;
                objectsList[idx].lgen.SparseUpdate();
                sparseUpdateCounter++;
            }
        }
    }

    GeneratorObject Create(LSystem lsys)
    {
        string shape = lsys.shape;

        // lame code,, refactor!
        GameObject proto = shape1;
        if (shape == "2")
        {
            proto = shape2;
        }
        if (shape == "3")
        {
            proto = shape3;
        }
        if (shape == "4")
        {
            proto = shape4;
        }
        if (shape == "5")
        {
            proto = shape5;
        }
        if (shape == "6")
        {
            proto = shape6;
        }
        if (shape == "7")
        {
            proto = shape7;
        }

        GameObject obj = Instantiate(proto, transform);
        obj.GetComponent<LGenerator>().SetLSystem(lsys);

        return new GeneratorObject(obj, shape);
    }

    public Vector3 CenterOfActivity()
    {
        float timeGate = 20.0f;

        float sumWeights = 0.0f;
        Vector3 sumPosition = new Vector3(0.0f, 0.0f, 0.0f);

        float ct = Time.time;

        foreach(GeneratorObject obj in objects.Values)
        {
            LGenerator lgen = obj.lgen;
            float dt = ct - lgen.lastTimeTouched;
            if(dt <= timeGate)
            {
                float weight = timeGate - dt;
                sumWeights += weight;
                sumPosition = sumPosition + (lgen.bounds.center * weight);
            }
        }

        if(sumWeights <= 5.0f)
        {
            sumWeights = 5.0f;
        }

        return sumPosition / sumWeights;
    }
}
