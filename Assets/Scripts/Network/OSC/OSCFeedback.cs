using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityOSC;

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

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGenerate(LSystemController lsysController)
    {
        OSCMessage msg = new OSCMessage("/ckartree");

        List<string> keyList = new List<string>(lsysController.forrest.Keys);
        keyList.Sort();

        foreach (string key in keyList)
        {
            LSystem lsys = lsysController.forrest[key];
            if (lsys.results.Count > 0) {
                string result = lsys.results[lsys.results.Count - 1];
                string dynamicsString = "";
                List<ProcessUnit> lastUnits = lsys.units[lsys.results.Count - 1];

                for(int i = 0; i < lastUnits.Count; i++)
                {
                    ProcessUnit unit = lastUnits[i];
                    dynamicsString += unit.Dynamic;
                    if (i != (lastUnits.Count-1))
                    {
                        dynamicsString += "|";
                    }
                }
                msg.Append(key + "@" + result + "A" + lsys.axiom + "G" + lsys.results.Count.ToString() + "S" + lsys.shape +  "D" + dynamicsString);
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

        msg.Append(ts.position[0]);
        msg.Append(ts.position[1]);
        msg.Append(ts.position[2]);

        msg.Append(ts.rotation[0]);
        msg.Append(ts.rotation[1]);
        msg.Append(ts.rotation[2]);

        msg.Append(ts.scale[0]);
        msg.Append(ts.scale[1]);
        msg.Append(ts.scale[2]);

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
