using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    private int port;
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
        Load();
        Commit();
    }

    void Load()
    {
        serverAddress = PlayerPrefs.GetString("serverAddress", GameObject.Find("OSCController").GetComponent<OSCController>().server.ToString());
        port = PlayerPrefs.GetInt("port", GameObject.Find("OSCController").GetComponent<OSCController>().port);

        sendToLocalhostToo = PlayerPrefs.GetInt("sendToLocalhostToo", GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo ? 1 : 0) != 0;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        dynamicsToSize = PlayerPrefs.GetInt("dynamicsToSize", lgen.dynamicsToSize ? 1 : 0) != 0;
        connectionSpheres = PlayerPrefs.GetInt("connectionSpheres", lgen.connectionSpheres ? 1 : 0) != 0;
        hideCubes = PlayerPrefs.GetInt("hideCubes", lgen.hideCubes ? 1 : 0) != 0;
        hideConnections = PlayerPrefs.GetInt("hideConnections", lgen.hideConnections ? 1 : 0) != 0;
        hideDynamics = PlayerPrefs.GetInt("hideDynamics", lgen.hideDynamics ? 1 : 0) != 0;
    }
    
    void Commit()
    {
        GameObject.Find("OSCController").GetComponent<OSCController>().server = serverAddress;
        GameObject.Find("OSCController").GetComponent<OSCController>().port = port;
        GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo = sendToLocalhostToo;
        GameObject.Find("OSCController").GetComponent<OSCController>().Connect();

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        lgen.dynamicsToSize = dynamicsToSize;
        lgen.connectionSpheres = connectionSpheres;
        lgen.hideCubes = hideCubes;
        lgen.hideConnections = hideConnections;
        lgen.hideDynamics = hideDynamics;

    }

    void ReadLocal()
    {
        serverAddress = GameObject.Find("OSCController").GetComponent<OSCController>().server.ToString();
        port = GameObject.Find("OSCController").GetComponent<OSCController>().port;

        sendToLocalhostToo = GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        dynamicsToSize = lgen.dynamicsToSize;
        connectionSpheres = lgen.connectionSpheres;
        hideCubes = lgen.hideCubes;
        hideConnections = lgen.hideConnections;
        hideDynamics = lgen.hideDynamics;
    }

    public void Save()
    {
        ReadLocal();

        PlayerPrefs.SetString("serverAddress", serverAddress);
        PlayerPrefs.SetInt("port", port);
        PlayerPrefs.SetInt("sendToLocalhostToo", sendToLocalhostToo ? 1 : 0);

        PlayerPrefs.SetInt("dynamicsToSize", dynamicsToSize ? 1 : 0);
        PlayerPrefs.SetInt("connectionSpheres", connectionSpheres ? 1 : 0);
        PlayerPrefs.SetInt("hideCubes", hideCubes ? 1 : 0);
        PlayerPrefs.SetInt("hideConnections", hideConnections ? 1 : 0);
        PlayerPrefs.SetInt("hideDynamics", hideDynamics ? 1 : 0);

        PlayerPrefs.Save();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
