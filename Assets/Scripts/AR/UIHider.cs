using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class UIHider : MonoBehaviour
{
    public GameObject ScreenshotButton;
    public GameObject CloseButton;
    public GameObject ResetPositionButton;

    public ARStatus ARStatus;

    private float lastTimeActive;

    [HideInInspector]
    public bool UIHidden = false;

    void Start()
    {
        lastTimeActive = Time.time;

        if(PersistentData.TestbedFakeAR)
        {
            UIHidden = false;
        }
    }

    void Update()
    {
        float timeToFadeout = 4f;
        float fadeoutTime = 1f;

        float now = Time.time;

        bool arStatusVisible = false;
        if(ARStatus != null)
        {
            arStatusVisible = ARStatus.StatusVisible;
        }

        if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) || arStatusVisible)
        {
            lastTimeActive = now;
            UIHidden = false;
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
            UIHidden = true;
        }

        ApplyAlpha(ScreenshotButton, alpha);
        ApplyAlpha(ResetPositionButton, alpha);
        ApplyAlpha(CloseButton, alpha);

    }

    private void ApplyAlpha(GameObject obj, float alpha)
    {
        if (obj)
        {
            Image img = obj.GetComponent<Image>();
            Color c = img.color;
            img.color = new Color(c.r, c.g, c.b, alpha);
        }
    }
}
