using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterTransformBehaviour : MonoBehaviour
{

    public bool invertTransform = false;
    public bool onlyTranslate = false;
    public bool worldIsAr = false;

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
        float direction = 1.0f;
        if(invertTransform)
        {
            direction = -1.0f;
        }

        gameObject.transform.localPosition = new Vector3(0.0f, 0.0f, 0.0f);

        if (!onlyTranslate)
        {
            gameObject.transform.localEulerAngles = new Vector3(ts.rotation[0] * direction, ts.rotation[1] * direction, ts.rotation[2] * direction);
        }

        if (!worldIsAr)
        {
            gameObject.transform.Translate(new Vector3(ts.position[0] * direction, ts.position[1] * direction, ts.position[2] * direction), Space.World);
        } else
        {   // rotated around x; y and z should switch
            gameObject.transform.Translate(new Vector3(ts.position[0] * direction, ts.position[2] * direction * -1.0f, ts.position[1] * direction* -1.0f), Space.World);

        }

        if (!onlyTranslate)
        {
            gameObject.transform.localScale = new Vector3(ts.scale[0], ts.scale[1], ts.scale[2]);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
