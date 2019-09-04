using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{

    public static bool worldIsAR;
    public bool setWorldIsAR = false;

    public static bool connectToLocal;
    public bool setConnectToLocal = false;

    public static bool floatUp;
    public bool setFloatUp = false;

    public int setVsyncCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        worldIsAR = setWorldIsAR;
        connectToLocal = setConnectToLocal;
        floatUp = setFloatUp;

        QualitySettings.vSyncCount = setVsyncCount;

        Destroy(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
