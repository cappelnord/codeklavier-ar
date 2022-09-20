using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.App {
public class OrientationPortrait : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        Destroy(gameObject);
    }
}
}