using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Testbed {

public class SkyboxStateToggle : MonoBehaviour
{

    private int state = 0;
    private int numStates = 2;

    private Camera cam;

    // Start is called before the first frame update
    void Start()
    {
        cam = GetComponent<Camera>();
        cam.clearFlags = CameraClearFlags.Skybox;
    }

    void Update()
    {
        if (Input.GetKeyDown("s"))
        {
            ToggleState();
        }
    }

    void ToggleState()
    {
        state = (state + 1) % numStates;

        switch(state)
        {
            case 0:
                cam.clearFlags = CameraClearFlags.Skybox;
                break;
            case 1:
                cam.clearFlags = CameraClearFlags.SolidColor;
                break;
        }
    }
}
}