using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ARquatic.LSystem;


namespace ARquatic.App {
    [DefaultExecutionOrder(-100)]
    public class CKARPrefabController : MonoBehaviour
    {
        public string ForceStartBundledID = "";

        private Dictionary<string, BundledSpecData> specs = new Dictionary<string, BundledSpecData>();

        public void AddSpec(BundledSpecData data) {
            specs[data.bundledID] = data;
        }

        void Awake() {
            if(ForceStartBundledID != "") {
                PersistentData.IsBundledChannel = true;
                PersistentData.BundledID = ForceStartBundledID;
            }

            if(PersistentData.IsBundledChannel) {
                Destroy(transform.Find("Websocket").gameObject);
                GameObject bundledPlayer = transform.Find("BundledPlayer").gameObject;

                if(!specs.ContainsKey(PersistentData.BundledID)) {
                    // make something smart
                    Debug.LogError("Could not find a spec with the ID " + PersistentData.BundledID);
                    return;
                }

                BundledSpecData data = specs[PersistentData.BundledID];
                bundledPlayer.GetComponent<AudioSource>().clip = data.audio;

                AudioLogFeeder feeder = bundledPlayer.GetComponent<AudioLogFeeder>();

                feeder.LogFile = data.log;
                feeder.LogOffset = data.logOffset;
                bundledPlayer.GetComponent<AudioSource>().time = data.playbackPositionStart;

            } else {
                Destroy(transform.Find("BundledPlayer").gameObject);
            } 
        }

        public void Start() {
            if(PersistentData.IsBundledChannel) {
                EventManager.InvokeNetworkStateChange(new CKARNetworkState(CKARNetworkStateType.ConnectedToServer, "fake connection"));
            }
        }
    }
}