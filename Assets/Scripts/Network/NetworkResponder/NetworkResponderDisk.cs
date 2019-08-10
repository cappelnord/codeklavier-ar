using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkResponderDisk : NetworkResponder
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Handle(CKARNetworkState state)
    {
        // https://docs.unity3d.com/ScriptReference/Material.SetColor.html

        Renderer rend = GetComponent<Renderer>();
        rend.material.shader = Shader.Find("Standard");

        Debug.Log(state.type);

        switch (state.type)
        {
            case CKARNetworkStateType.ConnectingToMaster:
            case CKARNetworkStateType.ConnectingToServer:
            case CKARNetworkStateType.ConnectingToLocal:
            case CKARNetworkStateType.ConnectedToMaster:
                {
                    rend.material.SetColor("_Color", Color.yellow);
                    break;
                }
            case CKARNetworkStateType.ConnectedToServer:
                {
                    rend.material.SetColor("_Color", Color.green);
                    break;
                }

            case CKARNetworkStateType.FailedConnectingToMaster:
            case CKARNetworkStateType.DisconnectedFromServer:
                {
                    rend.material.SetColor("_Color", Color.red);
                    break;
                }
        }
    }
}
