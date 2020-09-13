using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKConsoleStatus : MonoBehaviour
{
    void OnEnable() => EventManager.OnConsoleStatus += Set;

    void OnDisable() => EventManager.OnConsoleStatus -= Set;

    public void Set(string status) => GetComponent<TextMesh>().text = status;

}
