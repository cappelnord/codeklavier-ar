using System;
using System.Collections;
using System.Text;
using System.Collections.Generic;
using UnityEngine;

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

        private AudioSource audioSource;

        private int currentIndex = 0;
        private JSONLogEntry[] entries = null;

        void Awake() {
            audioSource = GetComponent<AudioSource>();

            string logString = LogFile.ToString();
            JSONLogFile file = JsonUtility.FromJson<JSONLogFile>(logString);
            entries = file.data;

            audioSource.Play();
        }

        void Update()
        {
            if(!audioSource.isPlaying) return;
            if(entries == null) return;
            if(currentIndex >= entries.Length) return;
            
            float currentPlaybackTime = audioSource.timeSamples / (float) audioSource.clip.frequency;

            while(currentIndex < entries.Length && entries[currentIndex].time <= currentPlaybackTime + LogOffset) {
                try {
                    JSONProcessor.Process(entries[currentIndex].payload);
                    Debug.Log(currentPlaybackTime);

                } catch(Exception e) {
                    Debug.Log(entries[currentIndex].payload);
                    Debug.Log(e);
                }
                currentIndex++;
            }
        }
    }
}

