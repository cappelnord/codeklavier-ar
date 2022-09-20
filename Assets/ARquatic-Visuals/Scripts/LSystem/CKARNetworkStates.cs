namespace ARquatic.LSystem {

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
}