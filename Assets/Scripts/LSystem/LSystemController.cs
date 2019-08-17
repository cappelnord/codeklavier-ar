using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcessUnit
{
    public bool Processed;
    public char Content;
    public List<ProcessUnit> Children;
    public List<ProcessUnit> Parents;
    public long id;
    public int Dynamic;

    static long idCounter = 0;

    public ProcessUnit(bool _processed, char _content, int _dynamic)
    {
        Processed = _processed;
        Content = _content;
        Dynamic = _dynamic;

        Children = new List<ProcessUnit>();
        Parents = new List<ProcessUnit>();

        id = idCounter;
        idCounter++;
    }

    public void AddChild(ProcessUnit unit)
    {
        Children.Add(unit);
        unit.Parents.Add(this);
    }
}

public class RuleSet
{
    public string From;
    public string To;
    public int Touched;
    public int id; // for sorting
    public List<int> Dynamics;

    public RuleSet(string _from, string _to, int _id, List<int> _dynamics)
    {
        From = _from;
        To = _to;
        id = _id;
        Dynamics = _dynamics;
        Touched = 0;
    }
}

public class LSystem
{
    LSystemController lsysController;


    public string rulesString;
    public string axiom = "0";

    int recursionDepth = 7;
    int maxStringLength = 64;
    int defaultVelocity = 90;

    private int rulesIDCounter = 0;

    public Dictionary<string, RuleSet> rules;
    public List<string> results;
    public List<List<ProcessUnit>> units;
    public int generation = 0;
    public List<int> axiomDynamics;

    private string fullStateString;

    public TransformSpec transformSpec;
    public string shape;

    public string key;

    public LSystem(LSystemController _controller, string _key)
    {
        lsysController = _controller;
        key = _key;

        transformSpec = TransformSpec.Identity();
        shape = "1";

        string tempAxiom = axiom;
        Reset();

        results = new List<string>();
        units = new List<List<ProcessUnit>>();

        axiom = tempAxiom;

        Generate();
    }

    void Reset()
    {
        rules = new Dictionary<string, RuleSet>();
        axiom = "0";
        axiomDynamics = new List<int>();
        axiomDynamics.Add(defaultVelocity);
    }

    public string StateString()
    {
        return fullStateString;
    }

    public void Parse(string data)
    {
        rulesString = data;

        string[] items = data.Split(',');

        foreach (string rule in items)
        {
            string[] pair = rule.Split('.');
            string from = pair[0];
            string to = pair[1];

            if (to != "")
            {
                string[] toPair = to.Split('d');
                int myDefaultDynamic = defaultVelocity;

                to = toPair[0];

                string dynamics = "";
                if (toPair.Length > 1)
                {
                    dynamics = toPair[1];
                }

                List<int> dynamicsList = new List<int>();
                if (dynamics != "")
                {
                    string[] dynamicsPairs = dynamics.Split('|');

                    foreach (string s in dynamicsPairs)
                    {
                        myDefaultDynamic = System.Int32.Parse(s);
                        dynamicsList.Add(myDefaultDynamic);
                    }
                }

                while (dynamicsList.Count < to.Length)
                {
                    dynamicsList.Add(myDefaultDynamic);
                }


                if (from == "*")
                {
                    if (to == "0")
                    {
                        Reset();
                    }
                    else
                    {
                        axiom = to;
                        axiomDynamics = dynamicsList;
                    }
                }
                else if (from == to)
                {
                    rules.Remove(from);
                }
                else
                {
                    if (to != "" && from != "")
                    {
                        rules[from] = new RuleSet(from, to, rulesIDCounter, dynamicsList);
                        rulesIDCounter++;
                    }
                }
            }
        }
    }

    public List<RuleSet> SortedRules()
    {
        List<RuleSet> sortedRules = new List<RuleSet>(rules.Values);

        sortedRules.Sort(delegate (RuleSet a, RuleSet b) {
            return b.id - a.id;
        });

        return sortedRules;
    }

    public void Generate()
    {
        generation++;

        results.Clear();
        results.Add(axiom);

        units.Clear();
        List<ProcessUnit> axiomList = new List<ProcessUnit>();

        // wem must assume axiom is same length as dynamics
        for (int i = 0; i < axiom.Length; i++)
        {
            axiomList.Add(new ProcessUnit(true, axiom[i], axiomDynamics[i]));
        }

        units.Add(axiomList);

        List<RuleSet> sortedRules = SortedRules();

        for (int i = 0; i < recursionDepth; i++)
        {
            List<ProcessUnit> list = ProcessList(units[i], sortedRules);
            units.Add(list);

            if (list.Count > 0)
            {
                string res = ProcessString(list);
                results.Add(res);
            }
        }

        // generate fullStateString
        fullStateString = "";
        foreach(string result in results)
        {
            fullStateString = fullStateString + result;
        }

    }


    private string ProcessString(List<ProcessUnit> input)
    {
        string res = "";
        foreach (ProcessUnit unit in input)
        {
            res = res + unit.Content;
        }
        return res;
    }

    private List<ProcessUnit> ProcessList(List<ProcessUnit> input, List<RuleSet> sortedRules)
    {
        List<ProcessUnit> list = new List<ProcessUnit>();

        // do an identity copy
        foreach (ProcessUnit unit in input)
        {
            ProcessUnit copy = new ProcessUnit(false, unit.Content, unit.Dynamic);
            unit.AddChild(copy);
            list.Add(copy);
        }

        foreach (RuleSet rule in sortedRules)
        {
            int len = rule.From.Length;
            string from = rule.From;

            List<ProcessUnit> newList = new List<ProcessUnit>();
            int i = 0;
            while (i < list.Count)
            {
                // early exits
                if (i + len > list.Count)
                {
                    newList.Add(list[i]);
                    i++;
                    continue;
                }

                bool matches = true;
                for (int e = 0; e < len; e++)
                {
                    matches = matches && (from[e] == list[i + e].Content) && !list[i + e].Processed;
                }

                if (matches)
                {
                    rule.Touched = generation;

                    for (int f = 0; f < rule.To.Length; f++)
                    {
                        char toSymbol = rule.To[f];
                        int toDynamic = rule.Dynamics[f];

                        if (toSymbol != 'N')
                        {

                            // new dynamic should be average of fromDynamics and the toDynamic
                            int contributors = 1 + len;
                            int sum = toDynamic;
                            for (int e = 0; e < len; e++)
                            {
                                sum += list[i + e].Dynamic; // this should be our old Dynamics
                            }

                            ProcessUnit newUnit = new ProcessUnit(true, toSymbol, sum / contributors);

                            for (int e = 0; e < len; e++)
                            {
                                foreach (ProcessUnit parent in list[i + e].Parents)
                                {
                                    parent.Children.Remove(list[i + e]);
                                    parent.AddChild(newUnit);
                                }

                            }
                            newList.Add(newUnit);
                        }
                    }
                    i = i + len;
                }
                else
                {
                    newList.Add(list[i]);
                    i++;
                }
            }
            list = newList;
        }

        if (list.Count > maxStringLength)
        {
            return new List<ProcessUnit>();
        }
        else
        {
            return list;
        }
    }
}

public class TransformSpec
{
    public float[] position;
    public float[] scale;
    public float[] rotation;

    public TransformSpec(float[] _position, float[] _scale, float[] _rotation)
    {
        position = _position;
        scale = _scale;
        rotation = _rotation;
    }

    public static TransformSpec Identity()
    {
        float[] _pos = { 0.0f, 0.0f, 0.0f };
        float[] _sca = { 1.0f, 1.0f, 1.0f };
        float[] _rot = { 0.0f, 0.0f, 0.0f };
        return new TransformSpec(_pos, _sca, _rot);
    }
}


public class LSystemController : MonoBehaviour
{

    public char[] symbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    public Dictionary<string, LSystem> forrest;

    private static LSystemController instance;

    public static LSystemController Instance()
    {
        if(instance == null)
        {
            instance = GameObject.Find("LSystemController").GetComponent<LSystemController>();
        }
        return instance;
    }

    public void Dispatch(string data)
    {
        // Debug.Log(data);
        Parse(data);
        Generate();
    }

    public void DispatchShape(string key, string shape)
    {
        AssureTree(key);
        if (forrest[key].shape != shape)
        {
            forrest[key].shape = shape;
            EventManager.InvokeShapeChange(this);
        }
    }

    public void DispatchView(string tree)
    {
        EventManager.InvokeViewChange(tree);
    }

    public void DispatchTransform(string key, TransformSpec ts)
    {
        GetLSystem(key).transformSpec = ts;
        EventManager.InvokeTransformChange(key, ts);
    }

    public LSystem GetLSystem(string key)
    {
        AssureTree(key);
        return forrest[key];
    }

    public void AssureTree(string key)
    {
        if (!forrest.ContainsKey(key))
        {
            forrest[key] = new LSystem(this, key);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        forrest = new Dictionary<string, LSystem>();
        EventManager.InvokeViewChange("1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void Parse(string data)
    {
        string[] rules = data.Split('#');
        foreach(string rule in rules)
        {
            string[] pair = rule.Split('@');
            string key = pair[0];
            string lsys = pair[1];
            GetLSystem(key).Parse(lsys);
        }
    }

    void Generate()
    {
        foreach(string key in forrest.Keys)
        {
            forrest[key].Generate();
        }

        EventManager.InvokeGenerate(this);
    }
}
