﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {

public class MasterTransformBehaviour : MonoBehaviour
{

    public bool InvertTransform = false;
    public bool OnlyTranslate = false;

    void OnEnable()
    {
        EventManager.OnMasterTransform += SetTransform;
    }

    void OnDisable()
    {
        EventManager.OnMasterTransform -= SetTransform;
    }

    public void SetTransform(TransformSpec ts)
    {
        float direction = 1.0f;
        if(InvertTransform)
        {
            direction = -1.0f;
        }

        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        if (!OnlyTranslate)
        {
            gameObject.transform.localEulerAngles = new Vector3(ts.Rotation[0] * direction, ts.Rotation[1] * direction, ts.Rotation[2] * direction);
        }

        if (!Config.WorldIsAR)
        {
            gameObject.transform.Translate(new Vector3(ts.Position[0] * direction, ts.Position[1] * direction, ts.Position[2] * direction), Space.World);
        } else
        {   // rotated around x; y and z should switch
            gameObject.transform.Translate(new Vector3(ts.Position[0] * direction, ts.Position[2] * direction * -1.0f, ts.Position[1] * direction), Space.World);

        }

        if (!OnlyTranslate)
        {
            gameObject.transform.localScale = new Vector3(ts.Scale[0], ts.Scale[1], ts.Scale[2]);
        }
    }
}
}