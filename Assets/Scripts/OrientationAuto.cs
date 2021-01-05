using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationAuto : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Destroy(gameObject);
    }
}
