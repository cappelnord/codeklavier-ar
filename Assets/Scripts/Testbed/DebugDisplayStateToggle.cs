using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugDisplayStateToggle : MonoBehaviour
{
    public GameObject UnityCage;

    private int state = 0;
    private int numStates = 3;


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("m"))
        {
            ToggleState();
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
