using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldIsAR : MonoBehaviour
{

    public bool worldIsAR = false;

    protected static WorldIsAR instance;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;
    }

    public static bool Get()
    {
        if(instance == null)
        {
            return false;
        }

        return instance.worldIsAR;
    }
}
