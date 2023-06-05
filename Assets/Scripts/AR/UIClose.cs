using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;


namespace ARquatic.App {

public class UIClose : MonoBehaviour, IPointerClickHandler
{
    private bool transitionHasStarted = false;

    public AudioMixer Mixer;

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
        Close();
    }

    public void Close() {
        if (transitionHasStarted) return;

        transitionHasStarted = true;

        UnityGray.GetComponent<RawImage>().enabled = true;
        UnityGray.GetComponent<FadeRawImageAlpha>().Start();

        float fadeTime = 0.5f;
        StartCoroutine(FadeOutAudio(fadeTime));

        UnityGray.GetComponent<FadeRawImageAlpha>().StartFade(1f, fadeTime, delegate ()
        {
            SceneManager.LoadScene("MainMenu");
        });
    }

    private IEnumerator FadeOutAudio(float time) {
        float start;
        Mixer.GetFloat("Volume", out start);

        float startTime = Time.time;

        while(Time.time - time < startTime) {
            float deltaNormalized = Mathf.Clamp((Time.time - startTime) / time, 0f, 1f);
            float volume = Mathf.Lerp(start, -40f, deltaNormalized);
            Mixer.SetFloat("Volume", volume);
            yield return 0;
        }
    }
}
}