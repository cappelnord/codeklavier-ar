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
            Mathf.Sin(position.x + Time.time * 0.3f),
            Mathf.Sin(position.z + position.y + Time.time * 0.35f) * 0.1f,
            Mathf.Sin(position.y + position.y + Time.time * 0.33f) * 0.4f
       );
    }
}
