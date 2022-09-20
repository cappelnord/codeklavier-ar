using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.LSystem;


namespace ARquatic.Visualizer {

public class CKConsoleStatus : MonoBehaviour
{
    void OnEnable() => EventManager.OnConsoleStatus += Set;

    void OnDisable() => EventManager.OnConsoleStatus -= Set;

    public void Set(string status) => GetComponent<TextMesh>().text = status;

}
}