using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplayStateToggle : MonoBehaviour
{
    public GameObject UnityCage;
    public GameObject CenterMark;
    public StageRotation Rotation;

    private int state = 0;
    private int numStates = 3;

    private bool centerMark = true;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            ToggleState();
        }

        if (Input.GetKeyDown("c"))
        {
            centerMark = !centerMark;
            CenterMark.SetActive(centerMark);
        }

        if (Input.GetKeyDown("r"))
        {
            Rotation.enabled = !Rotation.enabled;
        }
    }

    private void ToggleState()
    {
        state = (state + 1) % numStates;

        switch(state)
        {
            case 0:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CoordinateCross"))
                {
                    obj.GetComponent<MeshRenderer>().enabled = false;
                }
                UnityCage.SetActive(false);
                break;
            case 1: // display cages
                UnityCage.SetActive(true);
                break;
            case 2:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CoordinateCross"))
                {
                    obj.GetComponent<MeshRenderer>().enabled = true;
                }
                break;
        }
    }
}
