using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkResponderUI : NetworkResponder
{

    public Sprite AllOK;
    public Sprite Pending;
    public Sprite Error;

    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = Pending;
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
                    img.sprite = Pending;
                    img.enabled = true;
                    break;
                }
            case CKARNetworkStateType.ConnectedToServer:
                {
                    img.sprite = AllOK;
                    img.enabled = false;
                    break;
                }

            case CKARNetworkStateType.FailedConnectingToMaster:
            case CKARNetworkStateType.DisconnectedFromServer:
                {
                    img.sprite = Error;
                    img.enabled = true;
                    break;
                }
        }
    }
}
