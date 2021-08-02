using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-500)]
public class Config : MonoBehaviour
{
    public static bool WorldIsAR;
    public bool SetWorldIsAR = false;

    public static bool ConnectToLocal;
    public bool SetConnectToLocal = false;

    public static bool FloatUp;
    public bool SetFloatUp = false;

    public int SetVsyncCount = -1;

    public static float SymbolDynamics;
    public float SetSymbolDynamics = 1.0f;

    public int TargetFramerate = -1;

    // Start is called before the first frame update
    void Start()
    {
        Application.targetFrameRate = TargetFramerate;

        WorldIsAR = SetWorldIsAR;
        ConnectToLocal = SetConnectToLocal;
        FloatUp = SetFloatUp;

        QualitySettings.vSyncCount = SetVsyncCount;

        SymbolDynamics = SetSymbolDynamics;

        Destroy(gameObject);
    }
}
