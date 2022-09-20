using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {
public class AGenerator : LGenerator
{
    public GameObject[] Prefabs = new GameObject[10];
    public GameObject NullObject;
    public GameObject DynamicsText;

    private Dictionary<char, GameObject> lookup = new Dictionary<char, GameObject>();

    // Start is called before the first frame update
    protected override void Start()
    {
        int i = 0;
        foreach(char symbol in LSystemController.Instance().Symbols)
        {
            lookup.Add(symbol, Prefabs[i]);
            i++;
        }

        lookup.Add('N', NullObject);
    }

    public override void Generate()
    {

        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float pos = 0.0f;

        for(int i = 0; i < lsys.Axiom.Length; i++)
        {
            char symbol = lsys.Axiom[i];
            GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
            obj.transform.localPosition += new Vector3(pos, 0.0f, 0.0f);
            obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
            pos += 1.0f;

            GameObject text = Object.Instantiate(DynamicsText, gameObject.transform);
            text.transform.localPosition = obj.transform.localPosition + new Vector3(0.0f, -0.5f, -2.0f);
            text.GetComponent<TextMesh>().text = lsys.AxiomDynamics[i].ToString();
        }
    }

    override public void ApplyTransformSpec() { }
}
}