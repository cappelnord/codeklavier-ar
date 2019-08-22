using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetsBehaviour : MonoBehaviour
{

    public float degreesPerSecond = 0.0f;
    public float targetScale = 0.4f;

    private bool isAlive = true;

    IIRFilter scaleFilter;

    public LGenerator gen;

    // Start is called before the first frame update
    void Start()
    {
        scaleFilter = new IIRFilter(0.0f, 0.02f);
    }

    // Update is called once per frame
    void Update()
    {
        transform.localEulerAngles = new Vector3(transform.localEulerAngles[0], transform.localEulerAngles[1] + degreesPerSecond * Time.deltaTime * gen.speedMultiplier, transform.localEulerAngles[2]);
        float currentScale = scaleFilter.Filter(targetScale);
        transform.GetChild(0).transform.localScale = new Vector3(currentScale, currentScale, currentScale);

        if(!isAlive && currentScale < 0.0002f)
        {
            Destroy(this);
        }

    }

    public void Die()
    {
        isAlive = false;
        targetScale = 0.0f;
    }
}
