using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIHider : MonoBehaviour
{
    public GameObject ScreenshotButton;
    public GameObject CloseButton;
    public GameObject ResetPositionButton;

    private float lastTimeActive;

    void Start()
    {
        lastTimeActive = Time.time;
    }

    void Update()
    {
        float timeToFadeout = 4f;
        float fadeoutTime = 1f;

        float now = Time.time;

        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
        {
            lastTimeActive = now;
        }

        float alpha = 0f;


        if(lastTimeActive > (now - timeToFadeout))
        {
            alpha = 1f;
        } else if(lastTimeActive > (now - timeToFadeout - fadeoutTime))
        {
            float timeEllapsed = now - lastTimeActive - timeToFadeout;
            alpha = 1f - (timeEllapsed / fadeoutTime);
        }
        else
        {
            alpha = 0f;
        }

        ApplyAlpha(ScreenshotButton, alpha);
        ApplyAlpha(ResetPositionButton, alpha);
        ApplyAlpha(CloseButton, alpha);

    }

    private void ApplyAlpha(GameObject obj, float alpha)
    {
        Image img = obj.GetComponent<Image>();
        Color c = img.color;
        img.color = new Color(c.r, c.g, c.b, alpha);
    }
}
