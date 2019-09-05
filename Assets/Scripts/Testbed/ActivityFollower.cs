using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFollower : MonoBehaviour
{
    public GeneratorHerd herd;
    public bool active;
    public float maxZoomOutFactor = 1.3333f;
    private IIRFilter3 filter;
    private IIRFilter fovFilter;

    private float minFOV;
    private float maxFOV;

    private Camera camera;

    // Start is called before the first frame update
    void Start()
    {
        camera = GetComponent<Camera>();

        minFOV = camera.fieldOfView;
        maxFOV = minFOV * maxZoomOutFactor;

        filter = new IIRFilter3(new Vector3(0.0f, 0.0f, 0.0f), 0.002f);
        fovFilter = new IIRFilter(minFOV, 0.001f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("f"))
        {
            active = !active;
        }

        Vector3 activityCenter;
        float calcFOV;
        if (active)
        {
            activityCenter = herd.CenterOfActivity();
            calcFOV = minFOV;
            float magHerdSize = herd.GetBounds().size.magnitude;
            calcFOV = CKARTools.LinLin(magHerdSize, 8.0f, 20.0f, minFOV, maxFOV);
        }
        else
        {
            activityCenter = new Vector3(0.0f, 0.0f, 0.0f);
            calcFOV = minFOV;
        }

        Vector3 filteredActivityCenter = filter.Filter(activityCenter);
        transform.LookAt(filteredActivityCenter, Vector3.up);
        camera.fieldOfView = fovFilter.Filter(calcFOV);
    }
}
