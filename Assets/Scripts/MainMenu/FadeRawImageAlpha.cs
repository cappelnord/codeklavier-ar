using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeRawImageAlpha : MonoBehaviour
{
    private RawImage rawImage;

    private Color color;

    private float fromAlpha = 1f;
    private float toAlpha = 1f;
    private float currentAlpha = 1f;

    private float fromTime;
    private float toTime;

    void Start()
    {
        rawImage = GetComponent<RawImage>();
        color = rawImage.color;

        fromTime = Time.time;
        toTime = Time.time;
    }

    void Update()
    {
        if(Time.time < toTime)
        {
            float lerpValue = 1f - (toTime - Time.time) / (toTime - fromTime);
            currentAlpha = Mathf.Lerp(fromAlpha, toAlpha, lerpValue);
            rawImage.color = new Color(color.r, color.g, color.b, currentAlpha);
        } else if(currentAlpha != toAlpha)
        {
            currentAlpha = toAlpha;
            rawImage.color = new Color(color.r, color.g, color.b, toAlpha);
        }
    }

    public void StartFade(float targetAlpha, float time)
    {
        fromAlpha = currentAlpha;
        toAlpha = targetAlpha;
        fromTime = Time.time;
        toTime = Time.time + time;
    }
}
