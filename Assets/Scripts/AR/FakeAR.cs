using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeAR : MonoBehaviour
{
    public GameObject CenterMark;
    public GameObject Options;
    public GameObject DebugDisplayState;
    public GameObject OverlayUI;
    public GameObject UI;
    public GameObject EventSystem;
    public StageRotation Rotation;


    // Start is called before the first frame update
    void Start()
    {
        if (PersistentData.TestbedFakeAR)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            Destroy(CenterMark);
            Destroy(Options);
            Destroy(DebugDisplayState);
            Destroy(OverlayUI);
            Rotation.enabled = true;
            UI.SetActive(true);
        } else
        {
            Destroy(EventSystem);
            Destroy(UI);
        }

        Destroy(gameObject);
    }
}
