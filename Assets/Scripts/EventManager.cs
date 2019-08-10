using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager
{

    public delegate void GenerateAction(LSystemController lsysController);
    public static event GenerateAction OnGenerate;

    public delegate void ShapeChangeAction(LSystemController lsysController);
    public static event ShapeChangeAction OnShapeChange;

    public delegate void ViewChangeAction(string tree);
    public static event ViewChangeAction OnViewChange;

    public delegate void TransformChangeAction(string tree, TransformSpec ts);
    public static event TransformChangeAction OnTransformChange;

    public delegate void ConsoleStatusAction(string msg);
    public static event ConsoleStatusAction OnConsoleStatus;

    public delegate void ConsoleAction(string msg);
    public static event ConsoleAction OnConsole;

    public delegate void NetworkStateChangeAction(CKARNetworkState state);
    public static event NetworkStateChangeAction OnNetworkStateChange;


    public static void InvokeGenerate(LSystemController lsysController)
    {
        if(OnGenerate != null)
        {
            OnGenerate(lsysController);
        }
    }

    public static void InvokeShapeChange(LSystemController lsysController)
    {
        if (OnShapeChange != null)
        {
            OnShapeChange(lsysController);
        }
    }

    public static void InvokeViewChange(string tree)
    {
        if (OnViewChange != null)
        {
            OnViewChange(tree);
        }
    }

    public static void InvokeTransformChange(string tree, TransformSpec ts)
    {
        if (OnTransformChange != null)
        {
            OnTransformChange(tree, ts);
        }
    }

    public static void InvokeConsoleStatus(string msg)
    {
        if (OnConsoleStatus != null)
        {
            OnConsoleStatus(msg);
        }
    }

    public static void InvokeConsole(string msg)
    {
        if (OnConsole != null)
        {
            OnConsole(msg);
        }
    }

    public static void InvokeNetworkStateChange(CKARNetworkState state)
    {
        if (OnNetworkStateChange != null)
        {
            OnNetworkStateChange(state);
        }
    }
}
