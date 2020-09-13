using System.Collections;
using System.Collections.Generic;
using System.Net;
using System;
using UnityEngine;

using UnityOSC;

public class OSCController : MonoBehaviour
{
    private OSCClient client;
    private OSCClient localhost;

    public string ServerIP = "192.168.1.111";
    public int Port = 57120;

    public bool SendToLocalhostToo;

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

        client = new OSCClient(IPAddress.Parse(ServerIP), Port);
        client.Connect();

        localhost = new OSCClient(IPAddress.Parse("127.0.0.1"), Port);
        localhost.Connect();
    }

    private void Close()
    {
        localhost.Close();
        client.Close();
    }

    public void Send(OSCPacket packet)
    {
        try
        {
            client.Send(packet);
            if (SendToLocalhostToo)
            {
                localhost.Send(packet);
            }
        } catch (Exception e)
        {
            Debug.Log(e);
        }
    }

    void OnApplicationQuit()
    {
        Close();
    }
}
