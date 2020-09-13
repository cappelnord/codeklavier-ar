using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public static bool WorldIsAR;
    public bool SetWorldIsAR = false;

    public static bool ConnectToLocal;
    public bool SetConnectToLocal = false;

    public static bool FloatUp;
    public bool SetFloatUp = false;

    public int SetVsyncCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        WorldIsAR = SetWorldIsAR;
        ConnectToLocal = SetConnectToLocal;
        FloatUp = SetFloatUp;

        QualitySettings.vSyncCount = SetVsyncCount;

        Destroy(gameObject);
    }
}
