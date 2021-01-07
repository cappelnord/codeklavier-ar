using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


public class FadeRawImageAlpha : MonoBehaviour
{
    public GameObject DeactivateOnZero;
    private UnityAction actionOnFinish = null;

    private RawImage rawImage;

    private Color color;

    private float fromAlpha = 1f;
    private float toAlpha = 1f;
    private float currentAlpha = 1f;

    private float fromTime;
    private float toTime;

    public void Start()
    {
        rawImage = GetComponent<RawImage>();
        if (rawImage.enabled)
        {
            color = rawImage.color;
            currentAlpha = color.a;
        }

        fromTime = Time.time;
        toTime = -1f;
    }

    void Update()
    {
        if (toTime < 0) return;

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


        if(Time.time >= toTime && actionOnFinish != null)
        {
            actionOnFinish();
            actionOnFinish = null;
        }

        if(Time.time >= toTime && currentAlpha == 0f && DeactivateOnZero != null)
        {
            DeactivateOnZero.SetActive(false);
        }
    }

    public void StartFade(float targetAlpha, float time, UnityAction action = null)
    {
        fromAlpha = currentAlpha;
        toAlpha = targetAlpha;
        fromTime = Time.time;
        toTime = Time.time + time;
        this.actionOnFinish = action;
    }
}
