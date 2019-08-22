using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetVSyncCount : MonoBehaviour
{

    public int vSyncCount = -1;

    // Start is called before the first frame update
    void Start()
    {
        QualitySettings.vSyncCount = vSyncCount;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
