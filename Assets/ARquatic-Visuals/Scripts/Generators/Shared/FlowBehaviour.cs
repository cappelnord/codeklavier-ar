using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


namespace ARquatic.Visuals {
public class FlowBehaviour : MonoBehaviour
{
    private Quaternion baseRotation;
    public float RotationMultiplier = 35f;

    public LGenerator Gen;

    private double phaseX = 0.0;
    private double phaseY = 0.0;
    private double phaseZ = 0.0;

    // Start is called before the first frame update
    void Start()
    {
        baseRotation = transform.localRotation;

        Vector3 cv = ARquaticEnvironment.Instance.CurrentValues(transform.position);
        phaseX = cv.x * 4.0;
        phaseY = cv.y * 4.0;
        phaseZ = cv.z * 4.0;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 cw = ARquaticEnvironment.Instance.CurrentWeights(transform.position);
        phaseX += cw.x * Time.deltaTime * Gen.SpeedMultiplier;
        phaseY += cw.y * Time.deltaTime * Gen.SpeedMultiplier;
        phaseZ += cw.z * Time.deltaTime * Gen.SpeedMultiplier;

        transform.localRotation = baseRotation * Quaternion.Euler(new Vector3(Mathf.Sin((float) phaseX), Mathf.Sin((float) phaseY), Mathf.Sin((float) phaseZ)) * Gen.SpeedAmplitude * RotationMultiplier);

        // transform.localRotation = baseRotation * Quaternion.Euler(ARquaticEnvironment.Instance.Current(transform.position) * RotationMultiplier * Gen.SpeedMultiplier);
    }
}
}