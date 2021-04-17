using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeBehaviour : MonoBehaviour
{

    public Vector3 TargetScale;
    public float GrowStartTime;
    public float GrowTime;

    private IIRFilter scaleFilter = new IIRFilter(0.0f, 0.02f);


    // Start is called before the first frame update
    virtual public void Start()
    {
        transform.localScale = Vector3.zero;
    }

    // Update is called once per frame
    virtual public void Update()
    {
        if (Time.time > GrowStartTime)
        {
            float scale = scaleFilter.Filter(1.0f);
            transform.localScale = TargetScale * scale;
        }
    }
}
