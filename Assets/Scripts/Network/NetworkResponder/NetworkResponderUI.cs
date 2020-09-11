using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NetworkResponderUI : NetworkResponder
{

    public Sprite allOK;
    public Sprite pending;
    public Sprite error;

    private Image img;

    // Start is called before the first frame update
    void Start()
    {
        img = GetComponent<Image>();
        img.sprite = pending;
    }

    // Update is called once per frame
    void Update()
    {

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
                    img.sprite = pending;
                    img.enabled = true;
                    break;
                }
            case CKARNetworkStateType.ConnectedToServer:
                {
                    img.sprite = allOK;
                    img.enabled = false;
                    break;
                }

            case CKARNetworkStateType.FailedConnectingToMaster:
            case CKARNetworkStateType.DisconnectedFromServer:
                {
                    img.sprite = error;
                    img.enabled = true;
                    break;
                }
        }
    }
}
