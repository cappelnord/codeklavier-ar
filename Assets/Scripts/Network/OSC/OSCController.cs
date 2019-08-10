using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

using UnityOSC;

public class OSCController : MonoBehaviour
{
    private OSCClient client;
    private OSCClient localhost;

    public string server = "192.168.1.111";
    public int port = 57120;

    public bool sendToLocalhostToo;

    // Start is called before the first frame update
    void Start()
    {
        Connect();
    }

    public void Connect()
    {
        if(client != null)
        {
            Close();
        }

        client = new OSCClient(IPAddress.Parse(server), port);
        client.Connect();

        localhost = new OSCClient(IPAddress.Parse("127.0.0.1"), port);
        localhost.Connect();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void Close()
    {
        localhost.Close();
        client.Close();
    }

    public void Send(OSCPacket packet)
    {
        client.Send(packet);
        if(sendToLocalhostToo)
        {
            localhost.Send(packet);
        }
    }

    void OnApplicationQuit()
    {
        Close();
    }
}
