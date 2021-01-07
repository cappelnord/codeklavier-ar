using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(100)]
public class StartFadeOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<FadeRawImageAlpha>().StartFade(0f, 0.5f);
    }
}
