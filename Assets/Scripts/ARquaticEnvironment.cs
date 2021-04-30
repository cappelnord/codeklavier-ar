using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public Vector3 Current(Vector3 position)
    {
        // might be dumb ...
        return new Vector3(
            Mathf.Sin((position.x * 4.0f) + Time.time * 0.13f),
            Mathf.Sin((position.z + position.y) * 5.0f + Time.time * 0.135f) * 0.1f,
            Mathf.Sin((position.y + position.y) * 4.5f + Time.time * 0.133f) * 0.4f
       );
    }
}
