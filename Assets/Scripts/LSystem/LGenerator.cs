using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LGenerator : LSystemBehaviour
{

    public TransformSpec transformSpec;

    public System.Random rand;

    // Update is called once per frame
    void Update()
    {
        if (ShouldAct())
        {
            Generate();
        }
        if (lsys.transformSpec != transformSpec) {
            transformSpec = lsys.transformSpec;
            ApplyTransformSpec();
        }
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
        gameObject.transform.localScale = new Vector3(transformSpec.scale[0], transformSpec.scale[1], transformSpec.scale[2]);
        gameObject.transform.localEulerAngles = new Vector3(transformSpec.rotation[0], transformSpec.rotation[1], transformSpec.rotation[2]);
    }
}
