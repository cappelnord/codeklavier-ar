using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValueOscillator : MonoBehaviour
{

    public string Key = "1-intensity";
    public float Min = 0.0f;
    public float Max = 1.0f;
    public float Frequency = 1f;

    void Update()
    {
        ValueStore.Set(Key, CKARTools.LinLin(Mathf.Sin(Time.time * Frequency), -1f, 1f, Min, Max));
    }
}
