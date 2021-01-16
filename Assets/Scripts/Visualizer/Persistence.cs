using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class Persistence : MonoBehaviour
{

    private string _Channel;
    private bool _ConnectToLocal;
    private int _Port;
    private string _ServerIP;
    private bool _SendToLocalhostToo;

    private bool _FitToScreen = true;
    private bool _DynamicsToSize = true;
    private bool _ConnectionSpheres = true;
    private bool _HideCubes = false;
    private bool _HideConnections = false;
    private bool _HideDynamics = false;

    // TODO: Rewrite to directly query the component (not via GameObject names)

    // Start is called before the first frame update
    void Start()
    {
        Load();
        Commit();
    }

    void Load()
    {
        _Channel = PlayerPrefs.GetString("Channel", GameObject.Find("WebsocketController").GetComponent<WebsocketConsumer>().Channel);
        _ConnectToLocal = PlayerPrefs.GetInt("ConnectToLocal", GameObject.Find("Config").GetComponent<Config>().SetConnectToLocal ? 1 : 0) != 0;
        _ServerIP = PlayerPrefs.GetString("ServerIP", GameObject.Find("OSCController").GetComponent<OSCController>().ServerIP.ToString());
        _Port = PlayerPrefs.GetInt("Port", GameObject.Find("OSCController").GetComponent<OSCController>().Port);

        _SendToLocalhostToo = PlayerPrefs.GetInt("SendToLocalhostToo", GameObject.Find("OSCController").GetComponent<OSCController>().SendToLocalhostToo ? 1 : 0) != 0;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        _FitToScreen = PlayerPrefs.GetInt("FitToScreen", GameObject.Find("LGeneratorScaler").GetComponent<LGeneratorScaler>().Active ? 1 : 0) != 0;

        _DynamicsToSize = PlayerPrefs.GetInt("DynamicsToSize", lgen.DynamicsToSize ? 1 : 0) != 0;
        _ConnectionSpheres = PlayerPrefs.GetInt("ConnectionSpheres", lgen.ConnectionSpheres ? 1 : 0) != 0;
        _HideCubes = PlayerPrefs.GetInt("HideCubes", lgen.HideCubes ? 1 : 0) != 0;
        _HideConnections = PlayerPrefs.GetInt("HideConnections", lgen.HideConnections ? 1 : 0) != 0;
        _HideDynamics = PlayerPrefs.GetInt("HideDynamics", lgen.HideDynamics ? 1 : 0) != 0;
    }
    
    void Commit()
    {
        GameObject.Find("OSCController").GetComponent<OSCController>().ServerIP = _ServerIP;
        GameObject.Find("OSCController").GetComponent<OSCController>().Port = _Port;
        GameObject.Find("OSCController").GetComponent<OSCController>().SendToLocalhostToo = _SendToLocalhostToo;
        GameObject.Find("OSCController").GetComponent<OSCController>().Connect();

        GameObject.Find("LGeneratorScaler").GetComponent<LGeneratorScaler>().Active = _FitToScreen;

        GameObject.Find("WebsocketController").GetComponent<WebsocketConsumer>().Channel = _Channel;
        GameObject.Find("Config").GetComponent<Config>().SetConnectToLocal = _ConnectToLocal;

        LTestGenerator lgen = GameObject.Find("LGenerator").GetComponent<LTestGenerator>();

        lgen.DynamicsToSize = _DynamicsToSize;
        lgen.ConnectionSpheres = _ConnectionSpheres;
        lgen.HideCubes = _HideCubes;
        lgen.HideConnections = _HideConnections;
        lgen.HideDynamics = _HideDynamics;

    }

    void ReadLocal()
    {
        _Channel = GameObject.Find("WebsocketController").GetComponent<WebsocketConsumer>().Channel;
        _ConnectToLocal = Config.ConnectToLocal;

        _ServerIP = GameObject.Find("OSCController").GetComponent<OSCController>().ServerIP.ToString();
        _Port = GameObject.Find("OSCController").GetComponent<OSCController>().Port;

        _SendToLocalhostToo = GameObject.Find("OSCController").GetComponent<OSCController>().SendToLocalhostToo;
        _FitToScreen = GameObject.Find("LGeneratorScaler").GetComponent<LGeneratorScaler>().Active;

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

        PlayerPrefs.SetString("Channel", _Channel);
        PlayerPrefs.SetInt("ConnectToLocal", _ConnectToLocal ? 1 : 0);

        PlayerPrefs.SetString("ServerIP", _ServerIP);
        PlayerPrefs.SetInt("Port", _Port);
        PlayerPrefs.SetInt("SendToLocalhostToo", _SendToLocalhostToo ? 1 : 0);

        PlayerPrefs.SetInt("FitToScreen", _FitToScreen ? 1 : 0);

        PlayerPrefs.SetInt("DynamicsToSize", _DynamicsToSize ? 1 : 0);
        PlayerPrefs.SetInt("ConnectionSpheres", _ConnectionSpheres ? 1 : 0);
        PlayerPrefs.SetInt("HideCubes", _HideCubes ? 1 : 0);
        PlayerPrefs.SetInt("HideConnections", _HideConnections ? 1 : 0);
        PlayerPrefs.SetInt("HideDynamics", _HideDynamics ? 1 : 0);

        PlayerPrefs.Save();
    }
}
