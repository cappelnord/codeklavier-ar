using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleBehaviour : MonoBehaviour
{

    private bool lodged = true;
    private float startTime;
    private float scale = 0f;
    private float floatSpeed = 0.01f;

    public float TargetScale = 0.05f;

    public float TargetTime;

    public Transform TargetTransform;

    private float scaleRelativeToWorld;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {

        if(lodged)
        {
            float tn = Time.time;
            scale = Mathf.Lerp(0, TargetScale, (tn - startTime) / (TargetTime - startTime));
            transform.localScale = new Vector3(scale, scale, scale);

            if(Time.time > TargetTime)
            {
                Dislodge();
            }
        } else
        {
            floatSpeed = floatSpeed * 1.025f;
            transform.Translate((new Vector3(0f, floatSpeed, 0f) + ARquaticEnvironment.Instance.Current(transform.position) * 0.1f) * Time.deltaTime * scaleRelativeToWorld, Space.World);
            if(transform.localPosition.y > 5f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Dislodge()
    {
        transform.SetParent(TargetTransform);
        scaleRelativeToWorld = TargetTransform.lossyScale.x;
        lodged = false;
    }
}
