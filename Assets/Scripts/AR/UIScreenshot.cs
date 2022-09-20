using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace ARquatic.App {


public class UIScreenshot : MonoBehaviour, IPointerClickHandler
{

    public GameObject UI;

    public UIHider UIHider;
    public ScreenshotProvider ScreenshotProvider;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIHider.UIHidden) return;

        HideUI();
        // ScreenshotProvider.CaptureScreenshot((success, path) => ShowUI());
    }

    private void HideUI()
    {
        UI.GetComponent<Canvas>().enabled = false;
    }
    
    private void ShowUI()
    {
        UI.GetComponent<Canvas>().enabled = true;
    }
}
}
