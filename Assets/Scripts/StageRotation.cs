using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageRotation : MonoBehaviour
{
    public float DegreesPerSecond = 1.0f;

    void Update()
    {
        transform.Rotate(new Vector3(0.0f, DegreesPerSecond * Time.deltaTime, 0.0f), Space.Self);
    }
}
