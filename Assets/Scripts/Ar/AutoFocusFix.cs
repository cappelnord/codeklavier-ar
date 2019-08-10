using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoFocusFix : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Vuforia.CameraDevice.Instance.SetFocusMode(Vuforia.CameraDevice.FocusMode.FOCUS_MODE_CONTINUOUSAUTO);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
