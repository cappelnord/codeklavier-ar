using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTransformBehaviour : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    void OnEnable()
    {
        EventManager.OnMasterTransform += SetTransform;
    }

    void OnDisable()
    {
        EventManager.OnMasterTransform -= SetTransform;
    }

    public void SetTransform(TransformSpec ts)
    {
        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);
        gameObject.transform.localEulerAngles = new Vector3(ts.rotation[0], ts.rotation[1], ts.rotation[2]);
        gameObject.transform.Translate(new Vector3(ts.position[0], ts.position[1], ts.position[2]), Space.World);
        gameObject.transform.localScale = new Vector3(ts.scale[0], ts.scale[1], ts.scale[2]);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
