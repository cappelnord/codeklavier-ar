using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadingSpinner : MonoBehaviour
{
    public float DegreesPerSecond = 180f;

    void Update()
    {
        transform.Rotate(new Vector3(0f, 0f, Time.deltaTime * DegreesPerSecond));
    }
}
