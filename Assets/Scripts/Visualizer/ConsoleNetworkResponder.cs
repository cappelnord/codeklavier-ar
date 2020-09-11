using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConsoleNetworkResponder : NetworkResponder
{

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    override public void Handle(CKARNetworkState state)
    {
        EventManager.InvokeConsole(state.Message);
    }
}
