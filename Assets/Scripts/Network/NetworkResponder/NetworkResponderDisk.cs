using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkResponderDisk : NetworkResponder
{
    private Renderer myRenderer;

    void Start()
    {
     myRenderer = GetComponent<Renderer>();
     myRenderer.material.shader = Shader.Find("Standard");
    }

    public override void Handle(CKARNetworkState state)
    {
        switch (state.Type)
        {
            case CKARNetworkStateType.ConnectingToMaster:
            case CKARNetworkStateType.ConnectingToServer:
            case CKARNetworkStateType.ConnectingToLocal:
            case CKARNetworkStateType.ConnectedToMaster:
                {
                    myRenderer.material.SetColor("_Color", Color.yellow);
                    break;
                }
            case CKARNetworkStateType.ConnectedToServer:
                {
                    myRenderer.material.SetColor("_Color", Color.green);
                    break;
                }

            case CKARNetworkStateType.FailedConnectingToMaster:
            case CKARNetworkStateType.DisconnectedFromServer:
                {
                    myRenderer.material.SetColor("_Color", Color.red);
                    break;
                }
        }
    }
}
