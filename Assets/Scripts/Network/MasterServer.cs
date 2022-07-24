using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MasterServer
{
    public const string BaseURL = "https://ar.codeklavier.space/master/";
    public const int ClientProtocol = 1;
}

[Serializable]
public class MasterResponseChannelInfo
{
    public string status;
    public string name;
    public string description;
    public string eventISODate;
    public string eventURL;
    public string websocketBaseURL;
    public bool visible;
    public float baseDistance;
    public float baseScale;
    public float brightnessMultiplier;
    public bool nightMode;
}

[Serializable]
public class MasterResponseChannelInfoPair
{
    public string id;
    public MasterResponseChannelInfo info;
}

[Serializable]
public class MasterResponseAppInfo
{
    public int protocol;
    public MasterResponseChannelInfoPair[] channelList;
}