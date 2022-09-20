using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {
public class ARquaticEnvironment : MonoBehaviour
{
    public static ARquaticEnvironment Instance;


    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Vector3 CurrentValues(Vector3 position)
    {
        // might be dumb ...
        return new Vector3(
            Mathf.Sin((position.x * 4.0f) + Time.time * 0.121f),
            Mathf.Sin((position.z + position.y) * 5.0f + Time.time * 0.121f) * 0.1f,
            Mathf.Sin((position.y + position.y) * 4.5f + Time.time * 0.123f) * 0.4f
       );
    }

    public Vector3 CurrentWeights(Vector3 position)
    {
        // might be dumb ...
        return new Vector3(
            Mathf.Sin((position.x * 4.0f) + Time.time * 0.121f),
            Mathf.Sin((position.z + position.y) * 5.0f + Time.time * 0.121f) * 0.5f,
            Mathf.Sin((position.y + position.y) * 4.5f + Time.time * 0.123f) * 0.75f
       );
    }

}
}