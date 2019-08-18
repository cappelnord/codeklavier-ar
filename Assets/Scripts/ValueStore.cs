using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueStore
{
    private static Dictionary<string, float> dict;

    static ValueStore()
    {
        dict = new Dictionary<string, float>();
    }

    public static bool ContainsKey(string key)
    {
        return dict.ContainsKey(key);
    }

    public static float Get(string key)
    {
        return Get(key, 0.0f);
    }

    public static float Get(string key, float defaultValue)
    {
        if(dict.ContainsKey(key))
        {
            return dict[key];
        } else
        {
            // Debug.Log("ValueStore does not have a key: " + key);
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
