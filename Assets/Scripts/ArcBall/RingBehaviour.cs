using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingBehaviour : MonoBehaviour
{
    public Vector3 rotation;

    public float targetScale;
    public float scale;

    private bool destruct;

    public LGenerator gen;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(rotation * Time.deltaTime * gen.speedMultiplier);
        float dtWeight = Mathf.Clamp(0.05f * 60.0f * Time.deltaTime, 0.0f, 1.0f);

        scale = (scale * (1.0f - dtWeight)) + (targetScale * dtWeight);
        transform.localScale = new Vector3(scale, scale, scale);

        if(destruct)
        {
            if(scale < 0.01f)
            {
                GameObject.Destroy(gameObject);
            }
        }
    }

    public void Die()
    {
        destruct = true;
        targetScale = -5.0f;
    }
}
