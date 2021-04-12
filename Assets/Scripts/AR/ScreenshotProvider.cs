using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenshotProvider : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    /*

    public void CaptureScreenshot(NativeGallery.MediaSaveCallback callback)
    {
        StartCoroutine(TakeScreenshotAndSave(callback));
    }

    private IEnumerator TakeScreenshotAndSave(NativeGallery.MediaSaveCallback callback)
    {
        yield return new WaitForEndOfFrame();

        Texture2D ss = new Texture2D(Screen.width, Screen.height, TextureFormat.RGB24, false);
        ss.ReadPixels(new Rect(0, 0, Screen.width, Screen.height), 0, 0);
        ss.Apply();

        string filename = $"screenshot-{System.DateTime.Now.ToString("yyyy-mm-dd-HH-mm-ss")}.png";

        // Save the screenshot to Gallery/Photos
        // NativeGallery.Permission permission = NativeGallery.SaveImageToGallery(ss, "ARquatic", "screenshot-.png", callback);

        // Debug.Log(filename);
        // Debug.Log("Permission result: " + permission);

        // To avoid memory leaks
        Destroy(ss);
    }

    */

}
