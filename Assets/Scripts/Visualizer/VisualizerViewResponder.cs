using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisualizerViewResponder : MonoBehaviour
{
    void OnEnable()
    {
        EventManager.OnViewChange += SwitchView;
    }

    void OnDisable()
    {
        EventManager.OnViewChange -= SwitchView;
    }

    public void SwitchView(string key)
    {
        GetComponent<TextMesh>().text = key;
        LSystem lsys = LSystemController.Instance().GetLSystem(key);
        GameObject.Find("LGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
        GameObject.Find("AGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
        GameObject.Find("RGenerator").GetComponent<LGenerator>().SetLSystem(lsys);
    }
}
