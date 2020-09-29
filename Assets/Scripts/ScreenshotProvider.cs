using System.Collections;
using System.Collections.Generic;
using UnityEngine;

interface IScreenshotProvider
{
    void CaptureScreenshot();
}

public class ScreenshotProvider
{
    static IScreenshotProvider instance = null;

    public static void CaptureScreenshot()
    {
        if(instance == null)
        {
#if UNITY_ANDROID
            GameObject temp = new GameObject("AndroidScreenshotProvider");
            temp.AddComponent<AndroidScreenshotProvider>();
            instance = temp.GetComponent<AndroidScreenshotProvider>();
#endif
        }


        if(instance != null)
        {
            instance.CaptureScreenshot();
        } else
        {
            Debug.Log("No ScreenshotProvider could be instanced.");
        }
    }
}
