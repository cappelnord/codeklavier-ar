using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


public class RingBehaviour : MonoBehaviour
{
    public Vector3 Rotation;
    public float TargetScale;
    public float Scale;

    private bool destruct;

    [HideInInspector]
    public LGenerator gen;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(Rotation * Time.deltaTime * gen.SpeedMultiplier);
        float dtWeight = Mathf.Clamp(0.05f * 60.0f * Time.deltaTime, 0.0f, 1.0f);

        Scale = (Scale * (1.0f - dtWeight)) + (TargetScale * dtWeight);
        transform.localScale = new Vector3(Scale, Scale, Scale);

        if(destruct)
        {
            if(Scale < 0.01f)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void Die()
    {
        destruct = true;
        TargetScale = -5.0f;
    }
}
