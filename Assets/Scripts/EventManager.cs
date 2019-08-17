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

    public delegate void MarkerTransformAction(string marker, TransformSpec ts);
    public static event MarkerTransformAction OnMarkerTransform;

    public delegate void MasterTransformAction(TransformSpec ts);
    public static event MasterTransformAction OnMasterTransform;

    public delegate void ConsoleStatusAction(string msg);
    public static event ConsoleStatusAction OnConsoleStatus;

    public delegate void ConsoleAction(string msg);
    public static event ConsoleAction OnConsole;

    public delegate void NetworkStateChangeAction(CKARNetworkState state);
    public static event NetworkStateChangeAction OnNetworkStateChange;

    public delegate void ServerEventEndMarkerConfigAction();
    public static event ServerEventEndMarkerConfigAction OnServerEventEndMarkerConfig;

    public delegate void ValueAction(string key, float value);
    public static event ValueAction OnValue;

    private static Dictionary<string, ValueAction> valueActions;

    static EventManager() {
        valueActions = new Dictionary<string, ValueAction>();
    }

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

    public static void InvokeMarkerTransform(string marker, TransformSpec ts)
    {
        if (OnMarkerTransform != null)
        {
            OnMarkerTransform(marker, ts);
        }
    }

    public static void InvokeMasterTransform(TransformSpec ts)
    {
        if (OnMasterTransform != null)
        {
            OnMasterTransform(ts);
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

    public static void InvokeServerEventEndMarkerConfig()
    {
        if (OnServerEventEndMarkerConfig != null)
        {
            OnServerEventEndMarkerConfig();
        }
    }

    public static void SubscribeValue(string key, ValueAction action)
    {
        if(!valueActions.ContainsKey(key))
        {
            valueActions[key] = null;
        }
        (valueActions[key]) += action;
    }

    public static void UnsubscribeValue(string key, ValueAction action)
    {
        if (!valueActions.ContainsKey(key))
        {
            valueActions[key] = null;
        }

        (valueActions[key]) -= action;
    }

    public static void InvokeValue(string key, float value)
    {
        if (OnValue != null)
        {
            OnValue(key, value);
        }

        if (valueActions.ContainsKey(key))
        {
            ValueAction action = valueActions[key];
            if (action != null)
            {
                action(key, value);
            }
        }
    }
}
