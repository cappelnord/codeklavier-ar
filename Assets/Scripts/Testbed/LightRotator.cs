using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightRotator : MonoBehaviour
{

    private Vector3 basePosition;
    private Quaternion baseRotation;
    private float baseIntensity;

    public float IntensityPhaseMul = 0.2f;
    public float IntensityPhaseOffset = 0.0f;
    public float IntensityAmplitude = 0.1f;

    public float ZPosPhaseMul = 0.3f;
    public float ZPosPhaseOffset = 0.0f;
    public float ZPosAmplitude = 2.0f;

    public float XPosPhaseMul = 0.3f;
    public float XPosPhaseOffset = 0.0f;
    public float XPosAmplitude = 2.0f;


    public float ZRotPhaseMul = 0.5f;
    public float ZRotPhaseOffset = 0.0f;
    public float ZRotAmplitude = 30.0f;

    public float XRotPhaseMul = 0.6f;
    public float XRotPhaseOffset = 0.0f;
    public float XRotAmplitude = 30.0f;

    // Start is called before the first frame update
    void Start()
    {
        basePosition = transform.localPosition;
        baseRotation = transform.localRotation;
        baseIntensity = GetComponent<Light>().intensity;
    }

    // Update is called once per frame
    void Update()
    {
        float time = Time.time;

        GetComponent<Light>().intensity = baseIntensity + (Mathf.Sin(time * IntensityPhaseMul + IntensityPhaseOffset) * IntensityAmplitude);

        transform.localPosition = basePosition + new Vector3(Mathf.Sin(time * XPosPhaseMul + XPosPhaseOffset) * XPosAmplitude, 0.0f, Mathf.Sin(time * ZPosPhaseMul + ZPosPhaseOffset) * ZPosAmplitude);
        transform.localRotation = baseRotation * Quaternion.Euler(new Vector3(Mathf.Sin(time * XRotPhaseMul + XRotPhaseOffset) * XRotAmplitude, 0.0f, Mathf.Sin(time * ZRotPhaseMul + ZRotPhaseOffset) * ZRotAmplitude));


    }
}
