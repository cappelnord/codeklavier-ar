using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {


public class EventManager
{
    // TODO: Can I use something like anonymous types here?

    public delegate void GenerateAction(LSystemController lsysController);
    public static event GenerateAction OnGenerate;
    public static void InvokeGenerate(LSystemController lsysController) => OnGenerate?.Invoke(lsysController);


    public delegate void ShapeChangeAction(LSystemController lsysController);
    public static event ShapeChangeAction OnShapeChange;
    public static void InvokeShapeChange(LSystemController lsysController) => OnShapeChange?.Invoke(lsysController);


    public delegate void ViewChangeAction(string tree);
    public static event ViewChangeAction OnViewChange;
    public static void InvokeViewChange(string tree) => OnViewChange?.Invoke(tree);


    public delegate void TransformChangeAction(string tree, TransformSpec ts);
    public static event TransformChangeAction OnTransformChange;
    public static void InvokeTransformChange(string tree, TransformSpec ts) => OnTransformChange?.Invoke(tree, ts);


    public delegate void MarkerTransformAction(string marker, TransformSpec ts);
    public static event MarkerTransformAction OnMarkerTransform;
    public static void InvokeMarkerTransform(string marker, TransformSpec ts) => OnMarkerTransform?.Invoke(marker, ts);


    public delegate void MasterTransformAction(TransformSpec ts);
    public static event MasterTransformAction OnMasterTransform;
    public static void InvokeMasterTransform(TransformSpec ts) => OnMasterTransform?.Invoke(ts);


    public delegate void ConsoleStatusAction(string msg);
    public static event ConsoleStatusAction OnConsoleStatus;
    public static void InvokeConsoleStatus(string msg) => OnConsoleStatus?.Invoke(msg);


    public delegate void ConsoleAction(string msg);
    public static event ConsoleAction OnConsole;
    public static void InvokeConsole(string msg) => OnConsole?.Invoke(msg);


    public delegate void NetworkStateChangeAction(CKARNetworkState state);
    public static event NetworkStateChangeAction OnNetworkStateChange;
    public static void InvokeNetworkStateChange(CKARNetworkState state) => OnNetworkStateChange?.Invoke(state);


    public delegate void ServerEventEndMarkerConfigAction();
    public static event ServerEventEndMarkerConfigAction OnServerEventEndMarkerConfig;
    public static void InvokeServerEventEndMarkerConfig() => OnServerEventEndMarkerConfig?.Invoke();


    public delegate void ValueAction(string key, float value);
    public static event ValueAction OnValue;

    private static Dictionary<string, ValueAction> valueActions = new Dictionary<string, ValueAction>();

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
        OnValue?.Invoke(key, value);

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
}