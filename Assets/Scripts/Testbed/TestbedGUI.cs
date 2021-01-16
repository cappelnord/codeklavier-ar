using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[DefaultExecutionOrder(-1000)]
public class TestbedGUI : MonoBehaviour
{
    private bool active = false;
    private bool visible = true;

    private string channel;
    private bool connectToLocal;

    private void Start()
    {
        channel = PlayerPrefs.GetString("TBChannel", GameObject.Find("Websocket").GetComponent<WebsocketConsumer>().Channel);
        connectToLocal = PlayerPrefs.GetInt("TBConnectToLocal", GameObject.Find("Config").GetComponent<Config>().SetConnectToLocal ? 1 : 0) != 0;

        GameObject.Find("Websocket").GetComponent<WebsocketConsumer>().Channel = channel;
        GameObject.Find("Config").GetComponent<Config>().SetConnectToLocal = connectToLocal;
        Config.ConnectToLocal = connectToLocal;
    }

    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        if (Input.GetKeyDown("o"))
        {
            ToggleVisibility();
        }
    }

    void ToggleVisibility() => visible = !visible;


    // Update is called once per frame
    void OnGUI()
    {
        if (!visible) return;

        int bw = 100;
        int h = 25;

        if (!active)
        {
            if (GUI.Button(new Rect(10, 10, bw, h), "Options"))
            {
                active = true;
            }
        }
        else
        {
            GUI.Box(new Rect(10, 10, 280, 180), "Options (Hide with 'O')");

            int y = 50;
            int lx = 30;
            int rx = 150;
            int lw = 150;
            int sp = 30;
            int tw = 110;

            GUI.Label(new Rect(lx, y, lw, h), "Channel");
            channel = GUI.TextField(new Rect(rx, y, tw, h), channel);
            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Try Connect Local");
            connectToLocal = GUI.Toggle(new Rect(rx, y, tw, h), connectToLocal, "");

            y += (int)((float)sp * 1.5f);

            if (GUI.Button(new Rect(lx, y, bw, h), "Apply"))
            {
                active = false;
                CommitChanges();
            }

            if (GUI.Button(new Rect(lx + 30 + bw, y, bw, h), "Cancel"))
            {
                active = false;
            }
        }
    }

    private void CommitChanges()
    {
        bool needsReload = channel != GameObject.Find("Websocket").GetComponent<WebsocketConsumer>().Channel || Config.ConnectToLocal != connectToLocal;

        PlayerPrefs.SetString("TBChannel", channel);
        PlayerPrefs.SetInt("TBConnectToLocal", connectToLocal ? 1 : 0);

        if (needsReload)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }
}
