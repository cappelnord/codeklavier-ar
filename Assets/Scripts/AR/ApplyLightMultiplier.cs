using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyLightMultiplier : MonoBehaviour
{
    public float BaseModifier = 1f;

    private void Awake()
    {
        if (PersistentData.NightMode)
        {
            Destroy(this);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        LightEstimationLight le = GetComponent<LightEstimationLight>();
        le.m_BrightnessMod = BaseModifier * PersistentData.BrightnessMultiplier;
    }
}
