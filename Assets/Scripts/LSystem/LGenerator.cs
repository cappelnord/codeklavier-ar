using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGenerator : LSystemBehaviour
{

    public TransformSpec transformSpec;

    public System.Random rand;

    public float scaleMultiplier = 1.0f;
    private float targetScaleMultiplier = 1.0f;

    void OnEnable()
    {
        EventManager.OnValue += OnValue;
    }

    void OnDisable()
    {
        EventManager.OnValue -= OnValue;
    }

    void OnValue(string key, float value)
    {
        if (lsys == null) return;

        if(key == lsys.key + "-vel")
        {
            targetScaleMultiplier = 0.25f + (value * 1.75f);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (ShouldAct())
        {
            Generate();
        }

        if (lsys.transformSpec != transformSpec) {
            transformSpec = lsys.transformSpec;
        }

        float dtCounterWeight = Mathf.Clamp((1.0f - 0.99f) * 60.0f * Time.deltaTime, 0.0f, 1.0f);
        float dtGrowWeight = 1.0f - dtCounterWeight;

        scaleMultiplier = scaleMultiplier * dtGrowWeight + targetScaleMultiplier * dtCounterWeight;

        ApplyTransformSpec();
    }

    public float RandomRange(float min, float max)
    {
        return (float) (min + (rand.NextDouble() * (max - min)));
    }

    public void ResetRandomness()
    {
        char[] chars = lsys.rulesString.ToCharArray();
        int sum = 0;
        for(int i = 0; i < chars.Length; i++)
        {
            sum = sum + chars[i];
        }
        Debug.Log(sum);
        rand = new System.Random(sum % 100000);
    }

    virtual public void Generate()
    {

    }

    virtual public void ApplyTransformSpec()
    {
        gameObject.transform.localPosition = new Vector3(transformSpec.position[0] * 3.0f, transformSpec.position[1] * 3.0f, transformSpec.position[2] * 3.0f);
        gameObject.transform.localScale = new Vector3(transformSpec.scale[0] * scaleMultiplier, transformSpec.scale[1] * scaleMultiplier, transformSpec.scale[2] * scaleMultiplier);
        gameObject.transform.localEulerAngles = new Vector3(transformSpec.rotation[0], transformSpec.rotation[1], transformSpec.rotation[2]);
    }
}
