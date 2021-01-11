using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;



public class UIScreenshot : MonoBehaviour, IPointerClickHandler
{

    public GameObject UI;

    public UIHider UIHider;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIHider.UIHidden) return;

        ScreenshotProvider.CaptureScreenshot();
        StartCoroutine(HideUI(0.5f));
    }

    private IEnumerator HideUI(float time)
    {
        UI.GetComponent<Canvas>().enabled = false;
        yield return new WaitForSeconds(time);
        UI.GetComponent<Canvas>().enabled = true;
    }
}
