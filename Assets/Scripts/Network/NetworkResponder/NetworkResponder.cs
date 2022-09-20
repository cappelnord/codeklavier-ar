using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.LSystem;

namespace ARquatic.App {

public class NetworkResponder : MonoBehaviour
{
    void OnEnable() => EventManager.OnNetworkStateChange += Handle;
    void OnDisable() => EventManager.OnNetworkStateChange -= Handle;

    public virtual void Handle(CKARNetworkState state)
    {

    }
}
}