﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using ARquatic.LSystem;

public class ConsoleNetworkResponder : NetworkResponder
{
    override public void Handle(CKARNetworkState state) => EventManager.InvokeConsole(state.Message);
}
