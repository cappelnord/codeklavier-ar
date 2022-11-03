using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.Visuals {
public class DeathBehaviour : MonoBehaviour
{

    public Vector3 Direction;
    public Vector3 Rotation;
    public float Velocity;
    public float WaitUntilShrink;
    public float ShrinkStartTime;
    public Transform FloorTransform;
    public float FloorDistance = 1f;

    public float ScaleRelativeToWorld = 1.0f;

    private float scale = 1.0f;
    private float up = 0.01f;
    private Vector3 startScale;


    // Start is called before the first frame update
    void Start()
    {
        startScale = transform.localScale;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 ThisDirection = Direction;

        if(FloorTransform != null) {
            float distanceToFloor = transform.position.y - FloorTransform.position.y;
            if(distanceToFloor < FloorDistance) {
                if(distanceToFloor < 0) distanceToFloor = 0f;
                ThisDirection.y = ThisDirection.y * (distanceToFloor / FloorDistance);
                up *= 1.005f;
            }
        }

        Vector3 GeneralDirection = (ThisDirection  * Velocity + new Vector3(0f, up, 0f));

        transform.Translate(GeneralDirection * Time.deltaTime * ScaleRelativeToWorld, Space.World);
        transform.Rotate(Rotation * Time.deltaTime);

        if (up < 0.7)
        {
            up *= 1.01f;
        }

        if(Time.time > ShrinkStartTime)
        {
            scale = scale * 0.994f;
            transform.localScale = startScale * scale;
            if(scale < 0.03f)
            {
                Destroy(gameObject);
            }
        }
    }
}
}