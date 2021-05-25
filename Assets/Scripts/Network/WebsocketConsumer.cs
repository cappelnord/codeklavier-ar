using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Collections;
using System.Net.WebSockets;
using UnityEngine;
using UnityEngine.Networking;
using System.Net;
using System.Net.Sockets;


public class SingleLinkNode<T>
{
    // Note; the Next member cannot be a property since
    // it participates in many CAS operations
    public SingleLinkNode<T> Next;
    public T Item;
}

public static class SyncMethods
{
    public static bool CAS<T>(ref T location, T comparand, T newValue) where T : class
    {
        // Using non generic version of Interlocked.CompareExchange to avoid problems with AOT compilation
        return ReferenceEquals(comparand, Interlocked.CompareExchange(ref location, newValue, comparand));
    }
}

public class LockFreeLinkPool<T>
{

    private SingleLinkNode<T> head;

    public LockFreeLinkPool()
    {
        head = new SingleLinkNode<T>();
    }

    public void Push(SingleLinkNode<T> newNode)
    {
        newNode.Item = default(T);
        do
        {
            newNode.Next = head.Next;
        } while (!SyncMethods.CAS<SingleLinkNode<T>>(ref head.Next, newNode.Next, newNode));
        return;
    }

    public bool Pop(out SingleLinkNode<T> node)
    {
        do
        {
            node = head.Next;
            if (node == null)
            {
                return false;
            }
        } while (!SyncMethods.CAS<SingleLinkNode<T>>(ref head.Next, node, node.Next));
        return true;
    }
}

public class LockFreeQueue<T>
{

    SingleLinkNode<T> head;
    SingleLinkNode<T> tail;
    LockFreeLinkPool<T> trash;

    public LockFreeQueue()
    {
        head = new SingleLinkNode<T>();
        tail = head;
        trash = new LockFreeLinkPool<T>();
    }

    public void Enqueue(T item)
    {
        SingleLinkNode<T> oldTail = null;
        SingleLinkNode<T> oldTailNext;

        SingleLinkNode<T> newNode;
        if (!trash.Pop(out newNode))
        {
            newNode = new SingleLinkNode<T>();
        }
        else
        {
            newNode.Next = null;
        }
        newNode.Item = item;

        bool newNodeWasAdded = false;
        while (!newNodeWasAdded)
        {
            oldTail = tail;
            oldTailNext = oldTail.Next;

            if (tail == oldTail)
            {
                if (oldTailNext == null)
                    newNodeWasAdded = SyncMethods.CAS<SingleLinkNode<T>>(ref tail.Next, null, newNode);
                else
                    SyncMethods.CAS<SingleLinkNode<T>>(ref tail, oldTail, oldTailNext);
            }
        }
        SyncMethods.CAS<SingleLinkNode<T>>(ref tail, oldTail, newNode);
    }

    public bool Dequeue(out T item)
    {
        item = default(T);
        SingleLinkNode<T> oldHead = null;

        bool haveAdvancedHead = false;
        while (!haveAdvancedHead)
        {

            oldHead = head;
            SingleLinkNode<T> oldTail = tail;
            SingleLinkNode<T> oldHeadNext = oldHead.Next;

            if (oldHead == head)
            {
                if (oldHead == oldTail)
                {
                    if (oldHeadNext == null)
                    {
                        return false;
                    }
                    SyncMethods.CAS<SingleLinkNode<T>>(ref tail, oldTail, oldHeadNext);
                }
                else
                {
                    item = oldHeadNext.Item;
                    haveAdvancedHead = SyncMethods.CAS<SingleLinkNode<T>>(ref head, oldHead, oldHeadNext);
                    if (haveAdvancedHead)
                    {
                        trash.Push(oldHead);
                    }
                }
            }
        }
        return true;
    }
}

// be aware: The field names must be in small case
// as otherwise they would not properly deserialize

[Serializable]
public class WebsocketJsonMessage
{
    public string type;
    public string payload;
}

[Serializable]
public class WebsocketJsonValue
{
    public string type;
    public string key;
    public float payload;
}

[Serializable]
public class WebsocketJsonShape
{
    public string type;
    public string tree;
    public string shape;
}


[Serializable]
public class WebsocketJsonTransform
{
    public string type;
    public string tree;
    public float[] position;
    public float[] scale;
    public float[] rotation;
}

public enum CKARNetworkStateType
{
    ConnectingToMaster,
    ConnectedToMaster,
    FailedConnectingToMaster,
    ConnectingToLocal,
    ConnectingToServer,
    ConnectedToServer,
    DisconnectedFromServer
}

public class CKARNetworkState
{
    public CKARNetworkStateType Type { get; private set; }
    public string Message { get; private set; }

    public CKARNetworkState(CKARNetworkStateType type, string message)
    {
        Type = type;
        Message = message;
    }
}

public class WebsocketConsumer : MonoBehaviour
{
    public string UriString = "";
    public string Channel = "public";

    private ClientWebSocket cws = null;
    private ArraySegment<byte> buf = new ArraySegment<byte>(new byte[4096]);

    public bool RequestConsole = false;
    public bool RequestViews = false;


    private bool connected = false;
    private LockFreeQueue<string> queue = new LockFreeQueue<string>();
    private LockFreeQueue<CKARNetworkState> stateQueue = new LockFreeQueue<CKARNetworkState>();
    private LSystemController lsysController;
    private bool connectToLocal;
    private bool keepLocal = false;
    private int localConnectionAttempts = 0;

    private Coroutine currentCoroutine;

    private string messageBuffer = null;



    void Start()
    {
        connectToLocal = Config.ConnectToLocal;
        lsysController = LSystemController.Instance();

        if(PersistentData.SelectedChannel != null)
        {
            Channel = PersistentData.SelectedChannel;
        }

        if(Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android)
        {
            connectToLocal = false;
        }

        if (connectToLocal)
        {
            string localIP;
            using (Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, 0))
            {
                socket.Connect("8.8.8.8", 65530);
                IPEndPoint endPoint = socket.LocalEndPoint as IPEndPoint;
                localIP = endPoint.Address.ToString();
            }

            UriString = $"ws://{localIP}:8081/ckar_consume";
        }

        if (UriString == "")
        {
            ConnectToMaster();
        }
    }


    // kind of trash ... but works well enough in combination with OnDestroy
    private void Disconnect()
    {
        if(currentCoroutine != null)
        {
            StopCoroutine(currentCoroutine);
            currentCoroutine = null;
        }

        if (cws != null)
        {
            try
            {
                cws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Resetting Connection", CancellationToken.None);
                cws = null;
            } catch { }
        }

        connected = false;
    }

    void ConnectToMaster()
    {
        ResponderHandle(new CKARNetworkState(CKARNetworkStateType.ConnectingToMaster, "Connecting to Master ..."));
        currentCoroutine = StartCoroutine(RetreiveWebsocketURI());
    }

    IEnumerator RetreiveWebsocketURI()
    {

        using (UnityWebRequest webRequest = UnityWebRequest.Get(new Uri($"{MasterServer.BaseURL}channel?id={Channel}")))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            try
            {
                if (webRequest.result == UnityWebRequest.Result.ConnectionError)
                {
                    throw new Exception($"Could not connect to Master server: {webRequest.error}");
                } else if(webRequest.responseCode == 404)
                {
                    throw new Exception($"Channel \"{Channel}\" does not seem to be known by the Master server!");
                }
                else
                {
                    MasterResponseChannelInfo response = JsonUtility.FromJson<MasterResponseChannelInfo>(webRequest.downloadHandler.text);
                    UriString = $"{response.websocketBaseURL}ckar_consume";
                    EventManager.InvokeConsole($"'{Channel}' - {response.name}");
                    if(response.status != "online")
                    {
                        EventManager.InvokeConsole("Channel is probably offline ...");
                    }

                    if(response.websocketBaseURL == "" || response.websocketBaseURL == null)
                    {
                        throw new Exception("Selected channel does not have a websocket base URL set ...");
                    }

                    ResponderHandle(new CKARNetworkState(CKARNetworkStateType.ConnectedToMaster, $"Retrieved URL from Master!"));

                }
            }
            catch (Exception e) {
                Debug.LogException(e);
                ResponderHandle(new CKARNetworkState(CKARNetworkStateType.FailedConnectingToMaster, e.Message));
            }
        }
    }

    void Update()
    {
        bool doTryConnect = true;
        doTryConnect = doTryConnect && !connected && UriString != "";
        if (cws != null)
        {
            doTryConnect = doTryConnect && (cws.State == WebSocketState.Closed || cws.State == WebSocketState.Aborted);
        }

        if (doTryConnect)
        {
            connected = true;
            cws = null;
            TryConnect();
        }

        CKARNetworkState state;
        while (stateQueue.Dequeue(out state))
        {
            EventManager.InvokeNetworkStateChange(state);
        }

        string msgString;
        while (queue.Dequeue(out msgString))
        {
            if (messageBuffer != null)
            {
                msgString = messageBuffer + msgString;
            }

            if (!msgString.EndsWith("}"))
            {
                messageBuffer = msgString;
                continue;
            }

            messageBuffer = null;

            // Debug.Log(msgString);
            if(msgString.Contains("\"type\": \"transform\""))
            {
                WebsocketJsonTransform msg = JsonUtility.FromJson<WebsocketJsonTransform>(msgString);
                TransformSpec ts = new TransformSpec(msg.position, msg.scale, msg.rotation);
                if(msg.tree.Contains("marker"))
                {
                    EventManager.InvokeMarkerTransform(msg.tree, ts);
                } else if(msg.tree.Contains("master"))
                {
                    EventManager.InvokeMasterTransform(ts);
                } else
                {
                    lsysController.DispatchTransform(msg.tree, ts);
                }
            }
            else if (msgString.Contains("\"type\": \"shape\""))
            {
                WebsocketJsonShape msg = JsonUtility.FromJson<WebsocketJsonShape>(msgString);
                lsysController.DispatchShape(msg.tree, msg.shape);
            }
            else if (msgString.Contains("\"type\": \"value\""))
            {
                WebsocketJsonValue msg = JsonUtility.FromJson<WebsocketJsonValue>(msgString);
                string[] keys = msg.key.Split(',');
                foreach (string key in keys)
                {
                    ValueStore.Set(key, msg.payload); // will also invoke event
                }
            }
            else
            {
                WebsocketJsonMessage msg = JsonUtility.FromJson<WebsocketJsonMessage>(msgString);

                if(msg == null)
                {
                    EventManager.InvokeConsole($"Could not decode JSON message ... {msgString}");
                    continue;
                }

                if (msg.type == "lsys")
                {
                    lsysController.Dispatch(msg.payload);
                }

                if (msg.type == "console")
                {
                    EventManager.InvokeConsole(msg.payload);
                }

                if (msg.type == "consoleStatus")
                {
                    EventManager.InvokeConsoleStatus(msg.payload);
                }

                if (msg.type == "view")
                {
                    EventManager.InvokeViewChange(msg.payload);
                }

                if (msg.type == "serverEvent")
                {
                    if(msg.payload == "endMarkerConfig")
                    {
                        EventManager.InvokeServerEventEndMarkerConfig();
                    }
                }
            }
        }
    }

    void ResponderHandle(CKARNetworkState state)
    {
        stateQueue.Enqueue(state);
    }

    async void TryConnect()
    {
        connected = true;
        currentCoroutine = null;

        if (!connectToLocal)
        {
            ResponderHandle(new CKARNetworkState(CKARNetworkStateType.ConnectingToServer, "Connecting to Server ..."));
        } else
        {
            ResponderHandle(new CKARNetworkState(CKARNetworkStateType.ConnectingToLocal, "Connecting to Local ..."));
        }
        ClientWebSocket thisCWS = new ClientWebSocket();
        cws = thisCWS;

        try
        {
            Uri u = new Uri(UriString);
            await thisCWS.ConnectAsync(u, CancellationToken.None);

            if (connectToLocal) keepLocal = true;

            if(RequestConsole)
            {
                await SendString(thisCWS, "{\"type\": \"subscribe\", \"payload\": \"console\"}");
            }
            if(RequestViews)
            {
                await SendString(thisCWS, "{\"type\": \"subscribe\", \"payload\": \"view\"}");
            }
            ResponderHandle(new CKARNetworkState(CKARNetworkStateType.ConnectedToServer, "Connected to Server!"));
            GetStuff(thisCWS);
        }
        catch (WebSocketException e)
        {
            Debug.LogException(e);
            thisCWS.Abort();
            connected = false;
            TestSwitchToMaster();
        }
        catch (Exception e) {
            Debug.LogException(e);
            connected = false;
            TestSwitchToMaster();
        }
    }

    void TestSwitchToMaster()
    {
        if (connectToLocal && !keepLocal)
        {
            localConnectionAttempts++;
            if(localConnectionAttempts > 5)
            {
                connectToLocal = false;
                ConnectToMaster();
            }
        }
    }

    Task SendString(ClientWebSocket cws, string s)
    {
        try
        {
            ArraySegment<byte> b = new ArraySegment<byte>(Encoding.UTF8.GetBytes(s));
            return cws.SendAsync(b, WebSocketMessageType.Text, true, CancellationToken.None);
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            ResponderHandle(new CKARNetworkState(CKARNetworkStateType.DisconnectedFromServer, "Disconnected from Server!"));
            connected = false;
            return Task.FromResult(0);
        }
    }

    async void GetStuff(ClientWebSocket cws)
    {
        try
        {
            if (cws.State == WebSocketState.Open || cws.State == WebSocketState.CloseSent)
            {
                WebSocketReceiveResult r = await cws.ReceiveAsync(buf, CancellationToken.None);
                queue.Enqueue(Encoding.UTF8.GetString(buf.Array, 0, r.Count));
                GetStuff(cws);
            } 
        }
        catch (WebSocketException e)
        {
            Debug.LogException(e);
            cws.Abort();
            connected = false;
        }
        catch (Exception e)
        {
            Debug.LogException(e);
            connected = false;
        }
    }

    private void OnDestroy()
    {
        Debug.Log("WebsocketConsumer Destroy");
        Disconnect();
    }
}