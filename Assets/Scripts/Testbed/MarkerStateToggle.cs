using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MarkerStateToggle : MonoBehaviour
{

    private int state = 0;
    private int numStates = 4;

    public GameObject markerVisualizer;
    public GameObject unityCage;

    // Start is called before the first frame update
    void Start()
    {
        
    }

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
                foreach (Transform child in markerVisualizer.transform)
                {
                    child.gameObject.GetComponent<LoadMarkerTexture>().SetDisplayed(false);
                }
                unityCage.SetActive(false);
                break;
            case 1: // display markers
                foreach (Transform child in markerVisualizer.transform)
                {
                    child.gameObject.GetComponent<LoadMarkerTexture>().SetDisplayed(true);
                }
                break;
            case 2: // display cages
                unityCage.SetActive(true);
                break;
            case 3:
                foreach (GameObject obj in GameObject.FindGameObjectsWithTag("CoordinateCross"))
                {
                    obj.GetComponent<MeshRenderer>().enabled = true;
                }
                break;
        }
    }
}
