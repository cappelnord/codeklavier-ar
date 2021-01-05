using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrientationPortrait : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Destroy(gameObject);
    }
}
