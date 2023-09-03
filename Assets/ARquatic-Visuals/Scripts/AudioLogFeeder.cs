using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

using ARquatic.LSystem;

namespace ARquatic
{
    [Serializable]
    public class JSONLogFile {
        public JSONLogEntry[] data;
    }

    [Serializable]
    public class JSONLogEntry {
        public float time;
        public string payload;
    }


    [RequireComponent(typeof(AudioSource))]
    public class AudioLogFeeder : MonoBehaviour
    {
        public TextAsset LogFile;
        public JSONProcessor JSONProcessor;
        public float LogOffset = 0f;
        public bool PlayOnAwake = false;

        public AudioMixer Mixer;

        private AudioSource audioSource;

        private int currentIndex = 0;
        private JSONLogEntry[] entries = null;

        private float lastPlaybackTime = -1f;

        private bool isPlaying = false;

        private int framesUntilPlay = 5;

        public void Play() {
            if(isPlaying) return;
            if(framesUntilPlay > 0) return;

            if(Mixer != null) {
                Mixer.SetFloat("Volume", 0f);
            }
            audioSource.Play();
            isPlaying = true;
        }

        void Start() {
            audioSource = GetComponent<AudioSource>();

            string logString = LogFile.ToString();
            JSONLogFile file = JsonUtility.FromJson<JSONLogFile>(logString);
            entries = file.data;
        }

        private void AudioHasLooped() {
            currentIndex = 0;
            // TODO: We should probably reset it?
        }

        void Update()
        {
            framesUntilPlay--;
            if(framesUntilPlay > 0) return;

            if(PlayOnAwake) {
                Play();
            }

            if(entries == null) return;
            if(currentIndex >= entries.Length) return;
            
            float currentPlaybackTime = audioSource.timeSamples / (float) audioSource.clip.frequency;

            if(lastPlaybackTime > currentPlaybackTime) {
                AudioHasLooped();
            }

            lastPlaybackTime = currentPlaybackTime;

            while(currentIndex < entries.Length && entries[currentIndex].time <= currentPlaybackTime + LogOffset) {
                try {
                    JSONProcessor.Process(entries[currentIndex].payload);
                    // Debug.Log(currentPlaybackTime);

                } catch(Exception e) {
                    // Debug.Log(entries[currentIndex].payload);
                    // Debug.Log(e);
                }
                // Debug.Log(currentIndex);
                currentIndex++;
            }
        }
    }
}

