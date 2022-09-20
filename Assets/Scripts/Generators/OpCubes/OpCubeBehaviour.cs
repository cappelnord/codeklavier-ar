﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;
using ARquatic.Visuals;

namespace ARquatic.OldVisuals {

public class OpCubeBehaviour : MonoBehaviour
{

    public float DegreesPerSecond = 0.0f;
    public float TargetScale = 0.4f;

    private bool alive = true;

    private IIRFilter scaleFilter = new IIRFilter(0.0f, 0.02f);

    public LGenerator Gen;


    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1] + DegreesPerSecond * Time.deltaTime * Gen.SpeedMultiplier, transform.localEulerAngles[2]);
        float currentScale = scaleFilter.Filter(TargetScale);
        transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if (!alive && currentScale < 0.0002f)
        {
            Destroy(this);
        }

    }

    public void Die()
    {
        alive = false;
        TargetScale = 0.0f;
    }
}
}