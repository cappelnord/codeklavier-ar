using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionCube : MonoBehaviour
{

    public GameObject ARCamera;
    public GameObject ActualCube;

    void Start()
    {
        float scale = PersistentData.BaseScale * ResetPosition.DefaultScale;
        ActualCube.transform.localScale = new Vector3(scale, scale, scale);
    }

    // Update is called once per frame
    void Update()
    {
        // TODO: Find wall in the back and position in the middle
        Vector3 position = ARCamera.transform.localPosition + (ARCamera.transform.forward * PersistentData.BaseDistance);
        transform.localPosition = position;
        transform.localEulerAngles = new Vector3(0.0f, ARCamera.transform.localEulerAngles.y, 0.0f);
    }
}
