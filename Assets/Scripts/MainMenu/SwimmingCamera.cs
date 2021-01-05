using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwimmingCamera : MonoBehaviour
{

    private Vector3 position;
    private Quaternion rotation;

    // Start is called before the first frame update
    void Start()
    {
        position = transform.localPosition;
        rotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        float positionMul = 3f;
        float rotationMul = 1f;

        Vector3 deltaPosition = new Vector3(
            Mathf.Sin(Time.time * 0.02f + 0.3f) * 0.2f * positionMul,
            Mathf.Sin(Time.time * 0.04f + 1.5f) * 0.1f * positionMul,
            Mathf.Sin(Time.time * 0.013f + 2f) * 0.4f * positionMul
        );

        Vector3 deltaRotation = new Vector3(
               Mathf.Sin(Time.time * 0.014f - 0.3f) * 5f * rotationMul,
               Mathf.Sin(Time.time * 0.02f + 0.4f) * 3f * rotationMul,
               Mathf.Sin(Time.time * 0.012f + 1.7f) * 4f * rotationMul
        );

        transform.localPosition = position + deltaPosition;
        transform.rotation = rotation * Quaternion.Euler(deltaRotation);
    }
}
