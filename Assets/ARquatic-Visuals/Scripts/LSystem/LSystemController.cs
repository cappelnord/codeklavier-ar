using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {
public class ProcessUnit
{
    public bool Processed;
    public char Content;
    public List<ProcessUnit> Children;
    public List<ProcessUnit> Parents;
    public long Id;
    public int Dynamic;

    static long idCounter = 0;

    public ProcessUnit(bool processed, char content, int dynamic)
    {
        Processed = processed;
        Content = content;
        Dynamic = dynamic;

        Children = new List<ProcessUnit>();
        Parents = new List<ProcessUnit>();

        Id = idCounter;
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
    public int Id; 
    public List<int> Dynamics;

    public RuleSet(string _from, string _to, int _id, List<int> _dynamics)
    {
        From = _from;
        To = _to;
        Id = _id;
        Dynamics = _dynamics;
        Touched = 0;
    }
}

public class LSystem
{
    private const int maxRecursionDepth = 9;
    private const int defaultRecursionDepth = 1;
    private const int maxStringLength = 64;
    private const int defaultVelocity = 90;

    public string RulesString;
    public string Axiom = "0";

    public Dictionary<string, RuleSet> Rules;
    public List<string> Results;
    public List<List<ProcessUnit>> Units;
    public int Generation = 0;
    public List<int> AxiomDynamics;

    public TransformSpec TransformSpec;
    public string Shape;
    public string Key;

    public bool TooWide = false;

    public int RecursionDepth { get; private set; }
    private LSystemController lsysController;
    private int rulesIDCounter = 0;
    private string fullStateString;


    public LSystem(LSystemController controller, string key)
    {
        RecursionDepth = defaultRecursionDepth;

        lsysController = controller;
        Key = key;

        TransformSpec = TransformSpec.Identity();
        Shape = "1";

        Reset(Axiom);
        Results = new List<string>();
        Units = new List<List<ProcessUnit>>();

        Generate();
    }

    public void Reset(string axiom="0")
    {
        Rules = new Dictionary<string, RuleSet>();
        Axiom = axiom;
        AxiomDynamics = new List<int>();
        AxiomDynamics.Add(defaultVelocity);
    }

    public string StateString()
    {
        return fullStateString;
    }

    public void Parse(string data)
    {
        RulesString = data;

        string[] items = data.Split(',');

        foreach (string rule in items)
        {
            string[] pair = rule.Split('.');

            if(pair.Length < 2) {
                Debug.Log($"Invalid rule part: {rule}");
                continue;
            }

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
                        Axiom = to;
                        AxiomDynamics = dynamicsList;
                    }
                }
                else if (from == "g")
                {
                    int gSize = int.Parse(to);
                    if (gSize < 1) gSize = 1;
                    if (gSize > maxRecursionDepth) gSize = maxRecursionDepth;
                    RecursionDepth = gSize;
                }
                else if (from == to)
                {
                    Rules.Remove(from);
                }
                else
                {
                    if (to != "" && from != "")
                    {
                        Rules[from] = new RuleSet(from, to, rulesIDCounter, dynamicsList);
                        rulesIDCounter++;
                    }
                }
            } else {
                Debug.Log($"Invalid rule part: {rule}");
            }
        }
    }

    public List<RuleSet> SortedRules()
    {
        List<RuleSet> sortedRules = new List<RuleSet>(Rules.Values);

        sortedRules.Sort(delegate (RuleSet a, RuleSet b) {
            return b.Id - a.Id;
        });

        return sortedRules;
    }

    public void Generate()
    {
        Generation++;

        TooWide = false;

        Results.Clear();
        Results.Add(Axiom);

        Units.Clear();
        List<ProcessUnit> axiomList = new List<ProcessUnit>();

        // wem must assume axiom is same length as dynamics
        for (int i = 0; i < Axiom.Length; i++)
        {
            axiomList.Add(new ProcessUnit(true, Axiom[i], AxiomDynamics[i]));
        }

        Units.Add(axiomList);

        List<RuleSet> sortedRules = SortedRules();

        for (int i = 0; i < RecursionDepth; i++)
        {
            List<ProcessUnit> list = ProcessList(Units[i], sortedRules);
            Units.Add(list);

            if (list.Count > 0)
            {
                string res = ProcessString(list);
                Results.Add(res);
            }
        }

        fullStateString = "";

        foreach(string result in Results)
        {
            fullStateString = fullStateString + result;
        }

        fullStateString = fullStateString + "k:" + Key;

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
                    rule.Touched = Generation;

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
            TooWide = true;
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
    public float[] Position;
    public float[] Scale;
    public float[] Rotation;

    public TransformSpec(float[] position, float[] scale, float[] rotation)
    {
        Position = position;
        Scale = scale;
        Rotation = rotation;
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

    public char[] Symbols = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
    public Dictionary<string, LSystem> Forest;

    private static LSystemController instance;
    public static LSystemController Instance()
    {
        if(instance == null)
        {
            instance = GameObject.Find("LSystemController").GetComponent<LSystemController>();
        }
        return instance;
    }

    private bool Validate(string data)
    {
        // SHOULD BE PROPERLY IMPLEMENTED
        return data.Contains("@") && data.Contains(".");
    }

    public void Dispatch(string data)
    {
        // Debug.Log(data);
        if(!Validate(data))
        {
            Debug.Log($"Invalid lsys-rule: {data}");
            return;
        }

        try {

            Parse(data);
            Generate();

        } catch(Exception e) {
            Debug.Log(e);
        }
    }

    public void DispatchShape(string key, string shape)
    {
        AssureTree(key);
        if (Forest[key].Shape != shape)
        {
            Forest[key].Shape = shape;
            EventManager.InvokeShapeChange(this);
        }
    }

    public void DispatchTransform(string key, TransformSpec ts)
    {
        GetLSystem(key).TransformSpec = ts;
        EventManager.InvokeTransformChange(key, ts);
    }

    public LSystem GetLSystem(string key)
    {
        AssureTree(key);
        return Forest[key];
    }

    public void AssureTree(string key)
    {
        if (!Forest.ContainsKey(key))
        {
            Forest[key] = new LSystem(this, key);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ValueStore.Reset();
        Forest = new Dictionary<string, LSystem>();
        EventManager.InvokeViewChange("1");
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
        foreach(string key in Forest.Keys)
        {
            Forest[key].Generate();
        }

        EventManager.InvokeGenerate(this);
    }

    void OnDestroy()
    {
        instance = null;
    }
}
}