using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFollower : MonoBehaviour
{
    public GeneratorHerd herd;
    public bool active;
    private IIRFilter3 filter;


    // Start is called before the first frame update
    void Start()
    {
        filter = new IIRFilter3(new Vector3(0.0f, 0.0f, 0.0f), 0.002f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            active = !active;
        }

            Vector3 activityCenter;
        if (active)
        {
            activityCenter = herd.CenterOfActivity();
        } else
        {
            activityCenter = new Vector3(0.0f, 0.0f, 0.0f);
        }

        Vector3 filteredActivityCenter = filter.Filter(activityCenter);
        transform.LookAt(filteredActivityCenter, Vector3.up);
    }
}
