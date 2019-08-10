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
    }

    void OnDisable()
    {
        EventManager.OnGenerate -= OnGenerate;
        EventManager.OnShapeChange -= OnGenerate;
        EventManager.OnViewChange -= SendView;
        EventManager.OnTransformChange -= SendTransform;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnGenerate(LSystemController lsysController)
    {
        OSCMessage msg = new OSCMessage("/cktree");

        List<string> keyList = new List<string>(lsysController.forrest.Keys);
        keyList.Sort();

        foreach (string key in keyList)
        {
            LSystem lsys = lsysController.forrest[key];
            if (lsys.results.Count > 0) {
                string result = lsys.results[lsys.results.Count - 1];
                msg.Append(key + "@" + result + "A" + lsys.axiom + "G" + lsys.results.Count.ToString() + "S" + lsys.shape);
            }
        }

        osc.Send(msg);
    }

    public void SendView(string tree)
    {
        OSCMessage msg = new OSCMessage("/cktreeview");
        msg.Append(tree);
        osc.Send(msg);
    }

    public void SendTransform(string tree, TransformSpec ts)
    {
        OSCMessage msg = new OSCMessage("/cktreetransform");
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
}
