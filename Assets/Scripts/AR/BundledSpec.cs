using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.App {

    public struct BundledSpecData {
        public string bundledID;
        public AudioClip audio;
        public TextAsset log;
        public float logOffset;
    }

    [DefaultExecutionOrder(-1000)]
    public class BundledSpec : MonoBehaviour
    {
        public string BundledID;
        public AudioClip Audio;
        public TextAsset Log;
        public float LogOffset;

        public void Awake() {
            GetComponent<CKARPrefabController>().AddSpec(new BundledSpecData() {
                bundledID = BundledID,
                audio = Audio,
                log = Log,
                logOffset = LogOffset
            });

            Destroy(this);
        }
    }
}
