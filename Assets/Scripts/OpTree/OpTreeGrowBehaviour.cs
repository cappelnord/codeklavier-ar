using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpTreeGrowBehaviour : MonoBehaviour
{

    public Vector3 targetScale;
    public Vector3 targetRotation;
    public Vector3 targetPosition;

    public float growWeight = 0.99f;

    public Vector3 currentRotation;
    public Vector3 currentScale;
    public Vector3 currentPosition;

    public bool alive = true;

    private float windPhase;
    private float deltaWindPhase;

    public float windStrength = 0.02f;

    public float growDelay = 0.0f;
    private float startTime;

    public LGenerator gen;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;

        currentRotation = new Vector3(0.0f, 0.0f, 0.0f);
        currentScale = new Vector3(0.0f, 0.0f, 0.0f);
        currentPosition = new Vector3(0.0f, 0.0f, 0.0f);

        windPhase = gen.RandomRange(0.0f, Mathf.PI * 2.0f);
        deltaWindPhase = gen.RandomRange(0.003f, 0.007f) * 60.0f;
    }

    // Update is called once per frame
    void Update()
    {
        if (startTime + growDelay > Time.time) return;

        float dtCounterWeight = Mathf.Clamp((1.0f - growWeight) * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        float dtGrowWeight = 1.0f - dtCounterWeight;

        if (alive)
        {
            currentRotation = currentRotation * dtGrowWeight + targetRotation * dtCounterWeight;
            currentPosition = currentPosition * dtGrowWeight + targetPosition * dtCounterWeight;

            windPhase += (deltaWindPhase * Time.deltaTime);
            Vector3 windRotation = currentRotation * windStrength * Mathf.Sin(windPhase);

            transform.localEulerAngles = currentRotation + windRotation;
            transform.localPosition = currentPosition;
            currentScale = currentScale * dtGrowWeight + targetScale * dtCounterWeight;
            transform.localScale = currentScale;
        } else
        {
            currentScale = transform.localScale;

            float xzW = 1.0f - Mathf.Clamp(0.02f * 60.0f * Time.deltaTime, 0.0f, 1.0f);
            float yW =  1.0f - Mathf.Clamp(0.01f * 60.0f * Time.deltaTime, 0.0f, 1.0f);

            transform.localScale = new Vector3(currentScale[0] * xzW, currentScale[1] * yW, currentScale[2] * xzW);
          
            if(transform.localScale[1] < 0.01)
            {
                Destroy(gameObject);
            }
        }

    }

    public void Die()
    {
        alive = false;

        foreach (Transform child in transform)
        {
            child.gameObject.GetComponent<OpTreeGrowBehaviour>().Die();
        }

        // transform.parent = null;
    }
}
