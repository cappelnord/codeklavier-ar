using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;



public class UIClose : MonoBehaviour, IPointerClickHandler
{
    private bool transitionHasStarted = false;

    public GameObject UnityGray;

    public UIHider UIHider;

    void Start()
    {
        
        if(!PersistentData.FromMainMenu && !PersistentData.TestbedFakeAR)
        {
            gameObject.SetActive(false);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (UIHider.UIHidden) return;
        if (transitionHasStarted) return;

        transitionHasStarted = true;

        UnityGray.GetComponent<RawImage>().enabled = true;
        UnityGray.GetComponent<FadeRawImageAlpha>().Start();
        UnityGray.GetComponent<FadeRawImageAlpha>().StartFade(1f, 0.5f, delegate ()
        {
            SceneManager.LoadScene("MainMenu");
        });
    }
}
