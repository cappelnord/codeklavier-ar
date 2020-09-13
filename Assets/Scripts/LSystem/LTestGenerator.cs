using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LTestGenerator : LGenerator
{
    public GameObject[] Prefabs = new GameObject[10];

    public GameObject Connection;
    public GameObject ConnectionSphere;
    public GameObject DynamicsText;

    public bool DynamicsToSize = true;
    public bool ConnectionSpheres = true;
    public bool HideCubes = false;
    public bool HideConnections = false;
    public bool HideDynamics = false;

    private Dictionary<char, GameObject> lookup = new Dictionary<char, GameObject>();

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();

        int i = 0;
        foreach(char symbol in LSystemController.Instance().Symbols)
        {
            lookup.Add(symbol, Prefabs[i]);
            i++;
        }
    }

    public override void Generate()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float displace = 0.0f;

        Dictionary<long, Transform> transforms = new Dictionary<long, Transform>();

        foreach (List<ProcessUnit> data in lsys.Units)
        {

            float pos = data.Count / -2.0f;
            foreach(ProcessUnit unit in data)
            {
                char symbol = unit.Content;

                GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
                obj.transform.localPosition += new Vector3(pos, displace, 0.0f);
                obj.transform.localScale = new Vector3(0.0f, 0.0f, 0.0f);
                pos += 1.0f;

                if(DynamicsToSize && unit.Content != '0')
                {
                    obj.GetComponent<CubeGrower>().TargetSize = 0.25f + (0.5f * (unit.Dynamic / 128.0f));
                }

                if(HideCubes)
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }

                transforms[unit.Id] = obj.transform;
            }

            displace += 1.25f;
        }

        float connectionsZ = -0.3333f;

        foreach (List<ProcessUnit> data in lsys.Units)
        {
            foreach(ProcessUnit unit in data)
            {
                if(ConnectionSpheres && transforms.ContainsKey(unit.Id) && unit.Content != '0')
                {
                    GameObject obj = Object.Instantiate(ConnectionSphere, gameObject.transform);
                    obj.transform.localPosition = transforms[unit.Id].localPosition + new Vector3(0.0f, 0.0f, connectionsZ);
                    float scale = 0.25f + (unit.Dynamic / 128.0f * 0.5f);
                    obj.transform.localScale = new Vector3(scale, scale, scale);
                }

                foreach(ProcessUnit child in unit.Children)
                {
                    if(unit.Content != '0' && child.Content != '0') {
                        if(transforms.ContainsKey(unit.Id) && transforms.ContainsKey(child.Id)) {
                            GameObject obj = Object.Instantiate(Connection, gameObject.transform);

                            Transform fromTransform = transforms[unit.Id];
                            Transform toTransform = transforms[child.Id];

                            Vector3 between = toTransform.localPosition - fromTransform.localPosition;
                            float distance = between.magnitude;
                            obj.transform.localPosition = fromTransform.localPosition + (between * 0.5f) + new Vector3(0.0f, 0.0f, connectionsZ);
                            obj.transform.localScale = new Vector3(0.075f, 0.075f, distance);
                            obj.transform.localRotation = Quaternion.LookRotation(between);

                            if(HideConnections)
                            {
                                obj.GetComponent<MeshRenderer>().enabled = false;
                            }
                        }
                    }
                }
            }
        }

        if(!HideDynamics)
        {
            foreach (List<ProcessUnit> data in lsys.Units)
            {
                foreach(ProcessUnit unit in data)
                {
                    if(transforms.ContainsKey(unit.Id) && unit.Content != '0')
                    {
                        Transform transform = transforms[unit.Id];
                        GameObject obj = Object.Instantiate(DynamicsText, gameObject.transform);
                        obj.transform.localPosition = transform.localPosition + new Vector3(0.1f, -0.4f, -2.0f);
                        obj.GetComponent<TextMesh>().text = unit.Dynamic.ToString();

                    }
                }
            }
        }
    }

    override public void ApplyTransformSpec() { }
}
