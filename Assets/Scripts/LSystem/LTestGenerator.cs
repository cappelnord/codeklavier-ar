using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTestGenerator : LGenerator
{
    public GameObject[] prefabs = new GameObject[10];

    public GameObject connection;
    public GameObject connectionSphere;
    public GameObject dynamicsText;

    public bool dynamicsToSize = true;
    public bool connectionSpheres = true;
    public bool hideCubes = false;
    public bool hideConnections = false;
    public bool hideDynamics = false;

    private Dictionary<char, GameObject> lookup;

    // Start is called before the first frame update
    void Start()
    {
        base.Start();

        lookup = new Dictionary<char, GameObject>();

        int i = 0;
        foreach(char symbol in LSystemController.Instance().symbols)
        {
            lookup.Add(symbol, prefabs[i]);
            i++;
        }
    }

    override public void Generate()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float displace = 0.0f;

        Dictionary<long, Transform> transforms = new Dictionary<long, Transform>();

        foreach (List<ProcessUnit> data in lsys.units)
        {

            float pos = data.Count / -2.0f;
            foreach(ProcessUnit unit in data)
            {
                char symbol = unit.Content;

                GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
                obj.transform.localPosition += new Vector3(pos, displace, 0.0f);
                obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                pos += 1.0f;

                if(dynamicsToSize && unit.Content != '0')
                {
                    obj.GetComponent<CubeGrower>().targetSize = 0.25f + (0.5f * (unit.Dynamic / 128.0f));
                }

                if(hideCubes)
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }

                transforms[unit.id] = obj.transform;
            }

            displace += 1.25f;
        }

        float connectionsZ = -0.3333f;

        foreach (List<ProcessUnit> data in lsys.units)
        {
            foreach(ProcessUnit unit in data)
            {
                if(connectionSpheres && transforms.ContainsKey(unit.id) && unit.Content != '0')
                {
                    GameObject obj = Object.Instantiate(connectionSphere, gameObject.transform);
                    obj.transform.localPosition = transforms[unit.id].localPosition + new Vector3(0.0f, 0.0f, connectionsZ);
                    float scale = 0.25f + (unit.Dynamic / 128.0f * 0.5f);
                    obj.transform.localScale = new Vector3(scale, scale, scale);
                }

                foreach(ProcessUnit child in unit.Children)
                {
                    if(unit.Content != '0' && child.Content != '0') {
                        if(transforms.ContainsKey(unit.id) && transforms.ContainsKey(child.id)) {
                            GameObject obj = Object.Instantiate(connection, gameObject.transform);

                            Transform fromTransform = transforms[unit.id];
                            Transform toTransform = transforms[child.id];

                            Vector3 between = toTransform.localPosition - fromTransform.localPosition;
                            float distance = between.magnitude;
                            obj.transform.localPosition = fromTransform.localPosition + (between * 0.5f) + new Vector3(0.0f, 0.0f, connectionsZ);
                            obj.transform.localScale = new Vector3(0.075f, 0.075f, distance);
                            obj.transform.localRotation = Quaternion.LookRotation(between);

                            if(hideConnections)
                            {
                                obj.GetComponent<MeshRenderer>().enabled = false;
                            }
                        }
                    }
                }
            }
        }

        if(!hideDynamics)
        {
            foreach (List<ProcessUnit> data in lsys.units)
            {
                foreach(ProcessUnit unit in data)
                {
                    if(transforms.ContainsKey(unit.id) && unit.Content != '0')
                    {
                        Transform transform = transforms[unit.id];
                        GameObject obj = Object.Instantiate(dynamicsText, gameObject.transform);
                        obj.transform.localPosition = transform.localPosition + new Vector3(0.1f, -0.4f, -2.0f);
                        obj.GetComponent<TextMesh>().text = unit.Dynamic.ToString();

                    }
                }
            }
        }
    }

    override public void ApplyTransformSpec() { }
}
