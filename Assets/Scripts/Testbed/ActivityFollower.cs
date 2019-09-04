using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivityFollower : MonoBehaviour
{
    public GeneratorHerd herd;

    // Start is called before the first frame update
    void Start()
    {
        if(herd == null)
        {
            Debug.Log("ActivityFollower has no herd to follow!");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (herd == null)
        {
            return;
        }
    }
}
