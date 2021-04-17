using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowBehaviour : MonoBehaviour
{
    private Quaternion baseRotation;
    public float RotationMultiplier = 7f;

    // Start is called before the first frame update
    void Start()
    {
        baseRotation = transform.localRotation;
    }

    // Update is called once per frame
    void Update()
    {
        transform.localRotation = baseRotation * Quaternion.Euler(ARquaticEnvironment.Instance.Current(transform.position) * RotationMultiplier);
    }
}
