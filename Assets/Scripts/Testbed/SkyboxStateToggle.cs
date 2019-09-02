using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxStateToggle : MonoBehaviour
{

    private int state = 0;
    private int numStates = 2;

    public Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera.clearFlags = CameraClearFlags.Skybox;
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
                camera.clearFlags = CameraClearFlags.Skybox;
                break;
            case 1:
                camera.clearFlags = CameraClearFlags.SolidColor;
                break;
        }
    }
}
