using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;

namespace ARquatic.Visualizer {
public class VisualizerViewResponder : MonoBehaviour
{

    public string LockKey = "";

    private string key = "0";

    void OnEnable()
    {
        EventManager.OnViewChange += SwitchView;
        EventManager.OnGenerate += TestIfTooWide;
    }

    void OnDisable()
    {
        EventManager.OnViewChange -= SwitchView;
        EventManager.OnGenerate -= TestIfTooWide;
    }

    public void SwitchView(string newViewKey)
    {
        if(LockKey != "")
        {
            newViewKey = LockKey;
        }

        key = newViewKey;
        GetComponent<TextMesh>().text = key;
        var lsys = LSystemController.Instance().GetLSystem(key);
        GameObject.Find("LGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
        GameObject.Find("AGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
        GameObject.Find("RGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
    }

    public void TestIfTooWide(LSystemController controler)
    {
        var lsys = controler.GetLSystem(key);
        if(lsys.TooWide)
        {
            GetComponent<TextMesh>().color = Color.red;
        } else
        {
            GetComponent<TextMesh>().color = Color.white;
        }
    }
}
}