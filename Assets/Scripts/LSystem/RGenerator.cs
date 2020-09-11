using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RGenerator : LGenerator
{
    public GameObject[] prefabs = new GameObject[10];
    public GameObject seperator;
    public GameObject nullObject;
    public GameObject dynamicsText;

    private Dictionary<char, GameObject> lookup;

    // Start is called before the first frame update
    protected override void Start()
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

    // Update is called once per frame
    public void Update()
    {
        if(ShouldAct())
        {
            Generate();
        }
    }

    override public void Generate()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }

        float displace = 0.0f;

        List<RuleSet> rules = lsys.SortedRules();

        foreach (RuleSet rule in rules)
        {
            float pos = 0.0f;

            float alpha = 1.0f;

            if(rule.Touched != lsys.generation)
            {
                alpha = 0.2f;
            }

            foreach(char symbol in rule.From)
            {
                GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
                obj.transform.localPosition += new Vector3(pos, displace, 0.0f);
                obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                obj.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                pos += 1.0f;
            }

            GameObject sep = Object.Instantiate(seperator, gameObject.transform);
            sep.transform.localPosition += new Vector3(pos, displace, 0.0f);
            sep.transform.localScale = new Vector3(0.9f * 0.25f, 0.9f, 0.9f);
            sep.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);

            pos += 1.0f;

            for(int i = 0; i < rule.To.Length; i++)
            {
                char symbol = rule.To[i];
                GameObject obj = Object.Instantiate(lookup[symbol], gameObject.transform);
                obj.transform.localPosition += new Vector3(pos, displace, 0.0f);
                obj.transform.localScale = new Vector3(0.9f, 0.9f, 0.9f);
                obj.GetComponent<Renderer>().material.color = new Color(1.0f, 1.0f, 1.0f, alpha);
                pos += 1.0f;

                GameObject text = Object.Instantiate(dynamicsText, gameObject.transform);
                text.transform.localPosition = obj.transform.localPosition + new Vector3(0.0f, -0.5f, -2.0f);
                text.GetComponent<TextMesh>().text = rule.Dynamics[i].ToString();
            }

            displace -= 1.5f;
        }
        
    }

    override public void ApplyTransformSpec() { }
}
