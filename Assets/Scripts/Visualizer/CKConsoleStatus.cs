using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CKConsoleStatus : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable()
    {
        EventManager.OnConsoleStatus += Set;
    }

    void OnDisable()
    {
        EventManager.OnConsoleStatus -= Set;
    }

    public void Set(string s)
    {
        GetComponent<TextMesh>().text = s;
    }
}
