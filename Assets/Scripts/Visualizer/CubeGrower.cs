using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CubeGrower : MonoBehaviour
{

    public float targetSize = 0.75f;
    private float currentSize;
    
    // Start is called before the first frame update
    void Start()
    {
        currentSize = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        float dtWeight = Mathf.Clamp(0.15f * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        currentSize = (currentSize * (1.0f-dtWeight)) + (targetSize * dtWeight);
        transform.localScale = new Vector3(currentSize, currentSize, currentSize);

        if(transform.position.y <= -10.0f)
        {
            Destroy(gameObject);
        }
    }
}
