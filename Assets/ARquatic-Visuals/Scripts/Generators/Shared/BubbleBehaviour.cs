using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


namespace ARquatic.Visuals {
[DefaultExecutionOrder(200)]
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

    public LGenerator Gen;


    // Start is called before the first frame update
    void Start()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (WorldIsBoxed.Status)
        {
            return;
        }

        if (lodged)
        {
            float tn = Time.time;
            scale = Mathf.Lerp(0, TargetScale, (tn - startTime) / (TargetTime - startTime));
            scale = scale / Gen.ScaleMultiplier;

            Vector3 ps = transform.parent.transform.localScale;
            if(ps.x > 0.0f && ps.y > 0.0f && ps.z > 0.0f)
            {
                transform.localScale = new Vector3(scale / ps.x, scale / ps.y, scale / ps.z);
            }


            if (Time.time > TargetTime)
            {
                Dislodge();
            }
        } else
        {
            floatSpeed = floatSpeed * 1.025f;
            transform.Translate((new Vector3(0f, floatSpeed, 0f) + ARquaticEnvironment.Instance.CurrentValues(transform.position) * 0.1f) * Time.deltaTime * scaleRelativeToWorld, Space.World);
            if(transform.localPosition.y > 5f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Dislodge()
    {
        if (WorldIsBoxed.Status)
        {
            return;
        }

        transform.SetParent(TargetTransform);
        scaleRelativeToWorld = TargetTransform.lossyScale.x;
        lodged = false;
    }
}
}