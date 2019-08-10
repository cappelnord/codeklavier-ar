using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AGenerator : LGenerator
{
    public GameObject[] prefabs = new GameObject[10];
    public GameObject nullObject;
    public GameObject dynamicsText;

    private Dictionary<char, GameObject> lookup;

    // Start is called before the first frame update
    void Start()
    {

        lookup = new Dictionary<char, GameObject>();

        int i = 0;
        foreach(char symbol in LSystemController.Instance().symbols)
        {
            lookup.Add(symbol, prefabs[i]);
            i++;
        }

        lookup.Add('N', nullObject);
    }

    override public void Generate()
    {

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float pos = 0.0f;

        for(int i = 0; i < lsys.axiom.Length; i++)
        {
            char symbol = lsys.axiom[i];
            GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
            obj.transform.localPosition += new Vector3(pos, 0.0f, 0.0f);
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            pos += 1.0f;

            GameObject text = Object.Instantiate(dynamicsText, gameObject.transform);
            text.transform.localPosition = obj.transform.localPosition + new Vector3(0.0f, -0.5f, -2.0f);
            text.GetComponent<TextMesh>().text = lsys.axiomDynamics[i].ToString();
        }
    }

    override public void ApplyTransformSpec() { }

}
