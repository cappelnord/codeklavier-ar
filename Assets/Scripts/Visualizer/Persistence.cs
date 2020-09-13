using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Persistence : MonoBehaviour
{

    private int _Port;
    private string _ServerAddress;
    private bool _SendToLocalhostToo;

    private bool _DynamicsToSize = true;
    private bool _ConnectionSpheres = true;
    private bool _HideCubes = false;
    private bool _HideConnections = false;
    private bool _HideDynamics = false;

    // Start is called before the first frame update
    void Start()
    {
        Load();
        Commit();
    }

    void Load()
    {
        _ServerAddress = PlayerPrefs.GetString("ServerAddress", GameObject.Find("OSCController").GetComponent<OSCController>().server.ToString());
        _Port = PlayerPrefs.GetInt("Port", GameObject.Find("OSCController").GetComponent<OSCController>().port);

        _SendToLocalhostToo = PlayerPrefs.GetInt("SendToLocalhostToo", GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo ? 1 : 0) != 0;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        _DynamicsToSize = PlayerPrefs.GetInt("SynamicsToSize", lgen.DynamicsToSize ? 1 : 0) != 0;
        _ConnectionSpheres = PlayerPrefs.GetInt("ConnectionSpheres", lgen.ConnectionSpheres ? 1 : 0) != 0;
        _HideCubes = PlayerPrefs.GetInt("HideCubes", lgen.HideCubes ? 1 : 0) != 0;
        _HideConnections = PlayerPrefs.GetInt("HideConnections", lgen.HideConnections ? 1 : 0) != 0;
        _HideDynamics = PlayerPrefs.GetInt("HideDynamics", lgen.HideDynamics ? 1 : 0) != 0;
    }
    
    void Commit()
    {
        GameObject.Find("OSCController").GetComponent<OSCController>().server = _ServerAddress;
        GameObject.Find("OSCController").GetComponent<OSCController>().port = _Port;
        GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo = _SendToLocalhostToo;
        GameObject.Find("OSCController").GetComponent<OSCController>().Connect();

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        lgen.DynamicsToSize = _DynamicsToSize;
        lgen.ConnectionSpheres = _ConnectionSpheres;
        lgen.HideCubes = _HideCubes;
        lgen.HideConnections = _HideConnections;
        lgen.HideDynamics = _HideDynamics;

    }

    void ReadLocal()
    {
        _ServerAddress = GameObject.Find("OSCController").GetComponent<OSCController>().server.ToString();
        _Port = GameObject.Find("OSCController").GetComponent<OSCController>().port;

        _SendToLocalhostToo = GameObject.Find("OSCController").GetComponent<OSCController>().sendToLocalhostToo;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        _DynamicsToSize = lgen.DynamicsToSize;
        _ConnectionSpheres = lgen.ConnectionSpheres;
        _HideCubes = lgen.HideCubes;
        _HideConnections = lgen.HideConnections;
        _HideDynamics = lgen.HideDynamics;
    }

    public void Save()
    {
        ReadLocal();

        PlayerPrefs.SetString("ServerAddress", _ServerAddress);
        PlayerPrefs.SetInt("Port", _Port);
        PlayerPrefs.SetInt("SendToLocalhostToo", _SendToLocalhostToo ? 1 : 0);

        PlayerPrefs.SetInt("DynamicsToSize", _DynamicsToSize ? 1 : 0);
        PlayerPrefs.SetInt("ConnectionSpheres", _ConnectionSpheres ? 1 : 0);
        PlayerPrefs.SetInt("HideCubes", _HideCubes ? 1 : 0);
        PlayerPrefs.SetInt("HideConnections", _HideConnections ? 1 : 0);
        PlayerPrefs.SetInt("HideDynamics", _HideDynamics ? 1 : 0);

        PlayerPrefs.Save();
    }
}
