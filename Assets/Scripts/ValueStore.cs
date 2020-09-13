using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Rethink if this monolithicness is needed

public class ValueStore
{
    private static Dictionary<string, float> dict = new Dictionary<string, float>();

    public static bool ContainsKey(string key) => dict.ContainsKey(key);

    public static float Get(string key, float defaultValue=0.0f)
    {
        if (dict.ContainsKey(key))
        {
            return dict[key];
        }
        else
        {
            dict[key] = defaultValue;
            return defaultValue;
        }
    }

    public static void Set(string key, float value)
    {
        dict[key] = value;
        EventManager.InvokeValue(key, value);
    }
}
