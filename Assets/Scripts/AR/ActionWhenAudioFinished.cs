using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class ActionWhenAudioFinished : MonoBehaviour
{

    private bool hasBeenActivated = false;
    private bool hasBeenTriggered = false;
    private AudioSource audioSource;

    public UnityEvent OnAudioFinished;

    public void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void Update()
    {
        if(hasBeenTriggered) return;
        
        if(audioSource.isPlaying) {
            hasBeenActivated = true;
        }

        if(!audioSource.isPlaying && hasBeenActivated)
        {
            hasBeenTriggered = true;
            OnAudioFinished.Invoke();
        }
    }
}
