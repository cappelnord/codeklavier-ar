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
    }

    GeneratorObject Create(LSystem lsys)
    {
        string shape = lsys.shape;

        // lame
        GameObject proto = shape1;
        if (shape == "2")
        {
            proto = shape2;
        }
        if (shape == "3")
        {
            proto = shape3;
        }

        GameObject obj = Instantiate(proto, transform);
        obj.GetComponent<LSystemBehaviour>().SetLSystem(lsys);

        return new GeneratorObject(obj, shape);
    }
}
