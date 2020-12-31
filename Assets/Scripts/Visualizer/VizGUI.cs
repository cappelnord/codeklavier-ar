using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VizGUI : MonoBehaviour
{

    private bool active = false;
    private bool visible = true;

    private string portString;
    private string serverIP;
    private bool sendToLocalhostToo;

    private bool fitToScreen = true;
    private bool dynamicsToSize = true;
    private bool connectionSpheres = true;
    private bool hideCubes = false;
    private bool hideConnections = false;
    private bool hideDynamics = false;

    private string lockKey = "";


    // Update is called once per frame
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
                serverIP = GameObject.Find("OSCController").GetComponent<OSCController>().ServerIP.ToString();
                portString = GameObject.Find("OSCController").GetComponent<OSCController>().Port.ToString();
                sendToLocalhostToo = GameObject.Find("OSCController").GetComponent<OSCController>().SendToLocalhostToo;

                fitToScreen = GameObject.Find("LGeneratorScaler").GetComponent<LGeneratorScaler>().Active;

                LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();


                dynamicsToSize = lgen.DynamicsToSize;
                connectionSpheres = lgen.ConnectionSpheres;
                hideCubes = lgen.HideCubes;
                hideConnections = lgen.HideConnections;
                hideDynamics = lgen.HideDynamics;

                lockKey = GameObject.Find("VisualizerViewResponder").GetComponent<VisualizerViewResponder>().LockKey;

            }
            /*
            if (GUI.Button(new Rect(10 + bw + 30, 10, bw, h), "Exit"))
            {
                Application.Quit();
            }
            */
        }
        else
        {
            GUI.Box(new Rect(10, 10, 280, 430), "Options (Hide with 'O')");

            int y = 50;
            int lx = 30;
            int rx = 150;
            int lw = 150;
            int sp = 30;
            int tw = 110;


            GUI.Label(new Rect(lx, y, lw, h), "OSC server IP");
            serverIP = GUI.TextField(new Rect(rx, y, tw, h), serverIP);
            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "OSC port");
            portString = GUI.TextField(new Rect(rx, y, tw, h), portString);
            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Also to localhost");
            sendToLocalhostToo = GUI.Toggle(new Rect(rx, y, tw, h), sendToLocalhostToo, "");

            y += (int) ((float) sp * 1.5f);

            GUI.Label(new Rect(lx, y, lw, h), "Fit to screen");
            fitToScreen = GUI.Toggle(new Rect(rx, y, tw, h), fitToScreen, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Dynamics -> Size");
            dynamicsToSize = GUI.Toggle(new Rect(rx, y, tw, h), dynamicsToSize, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Spheres");
            connectionSpheres = GUI.Toggle(new Rect(rx, y, tw, h), connectionSpheres, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Hide Cubes");
            hideCubes = GUI.Toggle(new Rect(rx, y, tw, h), hideCubes, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Hide Lines");
            hideConnections = GUI.Toggle(new Rect(rx, y, tw, h), hideConnections, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Hide Dynamics");
            hideDynamics = GUI.Toggle(new Rect(rx, y, tw, h), hideDynamics, "");

            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Lock View to Key");
            lockKey = GUI.TextField(new Rect(rx, y, tw, h), lockKey);


            y += (int)((float)sp * 1.5f);

            if(GUI.Button(new Rect(lx, y, bw,h), "Apply"))
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
        OSCController oscc = GameObject.Find("OSCController").GetComponent<OSCController>();
        oscc.ServerIP = serverIP;
        oscc.Port = int.Parse(portString);
        oscc.SendToLocalhostToo = sendToLocalhostToo;

        oscc.Connect();

        GameObject.Find("LGeneratorScaler").GetComponent<LGeneratorScaler>().Active = fitToScreen;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        lgen.DynamicsToSize = dynamicsToSize;
        lgen.ConnectionSpheres = connectionSpheres;
        lgen.HideCubes = hideCubes;
        lgen.HideConnections = hideConnections;
        lgen.HideDynamics = hideDynamics;

        GameObject.Find("VisualizerViewResponder").GetComponent<VisualizerViewResponder>().LockKey = lockKey;
        GameObject.Find("VisualizerViewResponder").GetComponent<VisualizerViewResponder>().SwitchView(lockKey);

        lgen.Generate();

        GameObject.Find("Persistence").GetComponent<Persistence>().Save();
    }
}