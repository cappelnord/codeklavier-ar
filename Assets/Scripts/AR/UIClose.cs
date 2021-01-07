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

    void Start()
    {
        
        if(!PersistentData.FromMainMenu)
        {
            Destroy(gameObject);
        }
        
    }

    public void OnPointerClick(PointerEventData eventData)
    {
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
