using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VizGUI : MonoBehaviour
{

    private bool active = false;

    private string portString;
    private string serverAddress;
    private bool sendToLocalhostToo;

    private bool dynamicsToSize = true;
    private bool connectionSpheres = true;
    private bool hideCubes = false;
    private bool hideConnections = false;
    private bool hideDynamics = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void OnGUI()
    {
        int bw = 100;
        int h = 25;


        if (!active)
        {
            if (GUI.Button(new Rect(10, 10, bw, h), "Options"))
            {
                active = true;
                serverAddress = GameObject.Find("OSCController").GetComponent<OSCController>().server.ToString();
                portString = GameObject.Find("OSCController").GetComponent<OSCController>().port.ToString();
                sendToLocalhostToo = GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo;

                LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

                dynamicsToSize = lgen.DynamicsToSize;
                connectionSpheres = lgen.ConnectionSpheres;
                hideCubes = lgen.HideCubes;
                hideConnections = lgen.HideConnections;
                hideDynamics = lgen.HideDynamics;

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
            GUI.Box(new Rect(10, 10, 280, 360), "Options");

            int y = 50;
            int lx = 30;
            int rx = 150;
            int lw = 150;
            int sp = 30;
            int tw = 110;


            GUI.Label(new Rect(lx, y, lw, h), "OSC Send Server");
            serverAddress = GUI.TextField(new Rect(rx, y, tw, h), serverAddress);
            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "OSC Send Port");
            portString = GUI.TextField(new Rect(rx, y, tw, h), portString);
            y += sp;

            GUI.Label(new Rect(lx, y, lw, h), "Send to localhost");
            sendToLocalhostToo = GUI.Toggle(new Rect(rx, y, tw, h), sendToLocalhostToo, "");

            y += (int) ((float) sp * 1.5f);

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
        oscc.server = serverAddress;
        oscc.port = int.Parse(portString);
        oscc.sendToLocalhostToo = sendToLocalhostToo;

        oscc.Connect();

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        lgen.DynamicsToSize = dynamicsToSize;
        lgen.ConnectionSpheres = connectionSpheres;
        lgen.HideCubes = hideCubes;
        lgen.HideConnections = hideConnections;
        lgen.HideDynamics = hideDynamics;

        lgen.Generate();

        GameObject.Find("Persistence").GetComponent<Persistence>().Save();
    }
}