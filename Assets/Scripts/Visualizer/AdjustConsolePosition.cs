using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visualizer {

public class AdjustConsolePosition : MonoBehaviour
{
    void Update()
    {
        if(Camera.main.aspect > 1.7)
        {
            transform.localPosition = new Vector3(0.0f, 1.15f, -10.0f);
        }
        else
        {
            transform.localPosition = new Vector3(0.9f, 1.15f, -10.0f);
        }
    }
}
}