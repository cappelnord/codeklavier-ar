using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.App {

[DefaultExecutionOrder(-100)] 
public class ApplyLightMultiplier : MonoBehaviour
{
    public float BaseModifier = 1f;

    private void Awake()
    {
        if (PersistentData.NightMode)
        {
            Destroy(this);
        }

        LightEstimationLight le = GetComponent<LightEstimationLight>();
        le.m_BrightnessMod = BaseModifier * PersistentData.BrightnessMultiplier;
    }
}
}