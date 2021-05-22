using System;
using System.Net;
using System.Net.Sockets;
using UnityEngine;

public class IOSNetworkPermission
{
    //connect to broadcast address to send data and trigger network permission dialog in iOS 14
    public static void TriggerDialog()
    {
        //Main.Log("Trigger network dialog");
        var client = new TcpClient();
        try
        {
            client.Connect(IPAddress.Parse("255.255.255.255"), 8052);
        }
        catch (Exception e)
        {
            Debug.Log("IOSNetworkPermission exception occured");
            Debug.Log(e.Message);
        }
        client?.Close();
    }
}
