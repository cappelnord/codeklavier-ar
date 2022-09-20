using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.LSystem;
using ARquatic.App;

namespace ARquatic.Visualizer {

public class ConsoleNetworkResponder : NetworkResponder
{
    override public void Handle(CKARNetworkState state) => EventManager.InvokeConsole(state.Message);
}
}