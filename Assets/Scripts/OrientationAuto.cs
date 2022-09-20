using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.App {
public class OrientationAuto : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.AutoRotation;
        Destroy(gameObject);
    }
}
}