using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


using UnityOSC;
namespace ARquatic.App {


public class OSCFeedback : MonoBehaviour
{
    private OSCController osc;

    // Start is called before the first frame update
    void Start()
    {
        osc = GetComponent<OSCController>();
    }

    void OnEnable()
    {
        EventManager.OnGenerate += OnGenerate;
        EventManager.OnShapeChange += OnGenerate;
        EventManager.OnViewChange += SendView;
        EventManager.OnTransformChange += SendTransform;
        EventManager.OnValue += SendValue;
    }

    void OnDisable()
    {
        EventManager.OnGenerate -= OnGenerate;
        EventManager.OnShapeChange -= OnGenerate;
        EventManager.OnViewChange -= SendView;
        EventManager.OnTransformChange -= SendTransform;
        EventManager.OnValue -= SendValue;
    }

    public void OnGenerate(LSystemController lsysController)
    {
        OSCMessage msg = new OSCMessage("/ckartree");

        List<string> keyList = new List<string>(lsysController.Forest.Keys);
        keyList.Sort();

        foreach (string key in keyList)
        {
            var lsys = lsysController.Forest[key];
            int numResults = lsys.Results.Count;

            if (numResults > 0) {
                string result = lsys.Results[numResults - 1];
                string dynamicsString = "";
                List<ProcessUnit> lastUnits = lsys.Units[numResults - 1];

                for(int i = 0; i < lastUnits.Count; i++)
                {
                    ProcessUnit unit = lastUnits[i];
                    dynamicsString += unit.Dynamic;
                    if (i != (lastUnits.Count-1))
                    {
                        dynamicsString += "|";
                    }
                }
                msg.Append($"{key}@{result}A{lsys.Axiom}G{numResults}S{lsys.Shape}D{dynamicsString}");
            }
        }

        osc.Send(msg);
    }

    public void SendView(string tree)
    {
        OSCMessage msg = new OSCMessage("/ckartreeview");
        msg.Append(tree);
        osc.Send(msg);
    }

    public void SendTransform(string tree, TransformSpec ts)
    {
        OSCMessage msg = new OSCMessage("/ckartreetransform");
        msg.Append(tree);

        msg.Append(ts.Position[0]);
        msg.Append(ts.Position[1]);
        msg.Append(ts.Position[2]);

        msg.Append(ts.Rotation[0]);
        msg.Append(ts.Rotation[1]);
        msg.Append(ts.Rotation[2]);

        msg.Append(ts.Scale[0]);
        msg.Append(ts.Scale[1]);
        msg.Append(ts.Scale[2]);

        osc.Send(msg);
    }

    public void SendValue(string key, float value)
    {
        OSCMessage msg = new OSCMessage("/ckarvalue");
        msg.Append(key);
        msg.Append(value);

        osc.Send(msg);
    }
}
}