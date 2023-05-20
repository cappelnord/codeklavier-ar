using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ARquatic.LSystem {
    // be aware: The field names must be in small case
    // as otherwise they would not properly deserialize

    [Serializable]
    public class WebsocketJsonMessage
    {
        public string type;
        public string payload;
    }

    [Serializable]
    public class WebsocketJsonValue
    {
        public string type;
        public string key;
        public float payload;
    }

    [Serializable]
    public class WebsocketJsonShape
    {
        public string type;
        public string tree;
        public string shape;
    }


    [Serializable]
    public class WebsocketJsonTransform
    {
        public string type;
        public string tree;
        public float[] position;
        public float[] scale;
        public float[] rotation;
    }

    public class JSONProcessor : MonoBehaviour
    {

        private LSystemController lsysController;


        // Start is called before the first frame update
        void Start()
        {
            lsysController = LSystemController.Instance();
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        public void Process(string msgString) {

            // Debug.Log(msgString);
            if(msgString.Contains("\"type\": \"transform\""))
            {
                WebsocketJsonTransform msg = JsonUtility.FromJson<WebsocketJsonTransform>(msgString);
                TransformSpec ts = new TransformSpec(msg.position, msg.scale, msg.rotation);
                if(msg.tree.Contains("marker"))
                {
                    EventManager.InvokeMarkerTransform(msg.tree, ts);
                } else if(msg.tree.Contains("master"))
                {
                    EventManager.InvokeMasterTransform(ts);
                } else
                {
                    lsysController.DispatchTransform(msg.tree, ts);
                }
            }
            else if (msgString.Contains("\"type\": \"shape\""))
            {
                WebsocketJsonShape msg = JsonUtility.FromJson<WebsocketJsonShape>(msgString);
                lsysController.DispatchShape(msg.tree, msg.shape);
            }
            else if (msgString.Contains("\"type\": \"value\""))
            {
                WebsocketJsonValue msg = JsonUtility.FromJson<WebsocketJsonValue>(msgString);
                string[] keys = msg.key.Split(',');
                foreach (string key in keys)
                {
                    ValueStore.Set(key, msg.payload); // will also invoke event
                }
            }
            else
            {
                WebsocketJsonMessage msg = JsonUtility.FromJson<WebsocketJsonMessage>(msgString);

                if(msg == null)
                {
                    EventManager.InvokeConsole($"Could not decode JSON message ... {msgString}");
                    return;
                }

                if (msg.type == "lsys")
                {
                    Debug.Log($"LSys: {msg.payload}");
                    lsysController.Dispatch(msg.payload);
                }

                if (msg.type == "console")
                {
                    EventManager.InvokeConsole(msg.payload);
                }

                if (msg.type == "consoleStatus")
                {
                    EventManager.InvokeConsoleStatus(msg.payload);
                }

                if (msg.type == "view")
                {
                    EventManager.InvokeViewChange(msg.payload);
                }

                if (msg.type == "serverEvent")
                {
                    if(msg.payload == "endMarkerConfig")
                    {
                        EventManager.InvokeServerEventEndMarkerConfig();
                    }
                }
            }
        }
    }
}