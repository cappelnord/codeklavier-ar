using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;
using ARquatic.Visuals;

namespace ARquatic.OldVisuals {

public class OpTreeGrowBehaviour : MonoBehaviour
{

    public Vector3 TargetScale;
    public Vector3 TargetRotation;
    public Vector3 TargetPosition;

    public float GrowWeight = 0.99f;

    public Vector3 CurrentRotation;
    public Vector3 CurrentScale;
    public Vector3 CurrentPosition;


    public float WindStrength = 0.02f;
    public float GrowDelay = 0.0f;

    public LGenerator Gen;

    private bool alive = true;

    private float windPhase;
    private float deltaWindPhase;
    private float startTime;

    private IIRFilter windStrengthFilter;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        CurrentRotation = new Vector3(0.0f, 0.0f, 0.0f);
        CurrentScale = new Vector3(0.0f, 0.0f, 0.0f);
        CurrentPosition = new Vector3(0.0f, 0.0f, 0.0f);

        windPhase = Gen.RandomRange(0.0f, Mathf.PI * 2.0f);
        deltaWindPhase = Gen.RandomRange(0.003f, 0.007f) * 60.0f;

        windStrengthFilter = new IIRFilter(Gen.SpeedMultiplier, 0.01f);

    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + GrowDelay > Time.time) return;

        float dtCounterWeight = Mathf.Clamp((1.0f - GrowWeight) * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        float dtGrowWeight = 1.0f - dtCounterWeight;

        if (alive)
        {
            CurrentRotation = CurrentRotation * dtGrowWeight + TargetRotation * dtCounterWeight;
            CurrentPosition = CurrentPosition * dtGrowWeight + TargetPosition * dtCounterWeight;

            windPhase += (deltaWindPhase * Time.deltaTime * Gen.SpeedMultiplier);
            Vector3 windRotation = CurrentRotation * WindStrength * Mathf.Sin(windPhase) * windStrengthFilter.Filter(Gen.SpeedMultiplier);
             

            transform.localEulerAngles = CurrentRotation + windRotation;
            transform.localPosition = CurrentPosition;
            CurrentScale = CurrentScale * dtGrowWeight + TargetScale * dtCounterWeight;
            transform.localScale = CurrentScale;
        } else
        {
            CurrentScale = transform.localScale;

            float xzW = 1.0f - Mathf.Clamp(0.02f * 60.0f * Time.deltaTime, 0.0f, 1.0f);
            float yW =  1.0f - Mathf.Clamp(0.01f * 60.0f * Time.deltaTime, 0.0f, 1.0f);

            transform.localScale = new Vector3(CurrentScale[0] * xzW, CurrentScale[1] * yW, CurrentScale[2] * xzW);
          
            if(transform.localScale[1] < 0.001f)
            {
                Destroy(gameObject);
            }
        }

    }

    public void Die()
    {
        alive = false;

        Transform[] allChildren = GetComponentsInChildren<Transform>();
        foreach (Transform child in allChildren)
        {
            if (child == transform) continue;

            child.gameObject.GetComponent<OpTreeGrowBehaviour>().Die();
        }
    }
}
}