using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;

public class TestNetworking : SimpleNetworkedMonoBehavior
{
    public Text Text;

    private string lobbyIpAddress = "127.0.0.1";
    private string gimIpAddress = "127.0.0.1";
    private string lobbyPort = "15937";
    private string gicPort = "16000";
    private string gimPort = "16100";
    private string giPort = "16200";
    //private string clientPort = "16300";

    private string started = null;
    private GUIStyle guiStyle = new GUIStyle();
    private string summaryStatus = "Scalable Server";

    private ClientNetworked clientNetworked;
    private LobbyNetworked lobbyNetworked;
    private GameInstanceClusterNetworked gicNetworked;
    private GameInstanceManagerNetworked gameInstanceManagerNetworked;
    private GameInstanceNetworked gameInstanceNetworked;

    void Start()
    {
        DebugLog.SetText(Text);
        guiStyle.fontSize = 16;
        guiStyle.alignment = TextAnchor.UpperCenter;
    }

    void ShowSummaryStatus()
    {
        if (started == "Server")
        {
            summaryStatus = string.Format("Server - Lobby: {0}  MatchmakingAvailable {1}   GI Managers {2}  GI Available {3}",
                    lobbyNetworked.NumberPlayers,
                    lobbyNetworked.Matchmaking.NumberAvaialablePlayers,
                    gicNetworked.NumberGameInstanceManagers,
                    gicNetworked.AvailableGameInstances);
        }
        if (started == "GI Manager")
        {
            summaryStatus = string.Format("Game Instance Manager  GI {0}", gameInstanceManagerNetworked.AvailableGameInstances);
        }
        if (started == "GI")
        {
            summaryStatus = string.Format("Game Instance  Players {0}", gameInstanceNetworked.NumberPlayers);
        }
        if (started == "Client")
        {
            summaryStatus = string.Format("Client");
        }
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 30, 200, 30), summaryStatus, guiStyle);
    }

    void OnGUI()
    {
        ShowSummaryStatus();

        if (started == null)
        {
            //clientPort = GUI.TextField(new Rect(Screen.width - 300, 130, 100, 30), clientPort);


            if (GUI.Button(new Rect(10, 10, 120, 30), "Start Server"))
            {
                Debug.Log("Start Lobby  port: " + lobbyPort);
                started = "Server";
                StartServer(lobbyIpAddress, ushort.Parse(lobbyPort), ushort.Parse(gicPort));
            }
            GUI.Label(new Rect(110, 50, 100, 30), "IP Address", guiStyle);
            lobbyIpAddress = GUI.TextField(new Rect(10, 50, 90, 20), lobbyIpAddress);
            GUI.Label(new Rect(110, 80, 100, 20), "Lobby Port", guiStyle);
            lobbyPort = GUI.TextField(new Rect(10, 80, 90, 20), lobbyPort);
            GUI.Label(new Rect(110, 110, 100, 20), "GICluster Port", guiStyle);
            gicPort = GUI.TextField(new Rect(10, 110, 90, 20), gicPort);

            if (GUI.Button(new Rect(Screen.width - 210, 10, 190, 30), "Start GI Manager"))
            {
                Debug.Log("Start GI Manager");
                started = "GI Manager";
                StartGameInstanceManager(lobbyIpAddress, ushort.Parse(gicPort), ushort.Parse(gimPort));
            }
            gimIpAddress = GUI.TextField(new Rect(Screen.width - 210, 50, 90, 20), gimIpAddress);
            gimPort = GUI.TextField(new Rect(Screen.width - 210, 80, 90, 20), gimPort);
            GUI.Label(new Rect(Screen.width - 110, 50, 100, 20), "IP Address", guiStyle);
            GUI.Label(new Rect(Screen.width - 110, 80, 100, 20), "Port", guiStyle);

            if (GUI.Button(new Rect(Screen.width - 210, 150, 190, 30), "Start GI"))
            {
                Debug.Log("Start GI");
                started = "GI";
                StartGameInstance(gimIpAddress, ushort.Parse(gimPort));
            }
            giPort = GUI.TextField(new Rect(Screen.width - 210, 190, 90, 20), giPort);
            GUI.Label(new Rect(Screen.width - 110, 190, 100, 20), "Port", guiStyle);

            if (GUI.Button(new Rect(10, 150, 190, 30), "Start Client"))
            {
                DebugLog.Log("Start Client");
                started = "Client";
                StartClient(lobbyIpAddress, ushort.Parse(lobbyPort));
            }
        }
        else
        {
            if (started == "Client")
            {
                if (GUI.Button(new Rect(Screen.width - 200, 10, 190, 30), "Start Match"))
                {
                    //clientNetworked.RequestMatch();
                    RPC("RequestStartMatch", NetworkReceivers.Server, string.Format("NetworkID:{0}", clientNetworked.NetWorker.Uniqueidentifier));
                }
                if (GUI.Button(new Rect(Screen.width - 200, 50, 190, 30), "Disconnect"))
                {
                    clientNetworked.DisconnectClient();
                    started = null;
                }
            }
            if (started == "GI Manager")
            {
                if (GUI.Button(new Rect(Screen.width - 200, 10, 190, 30), "Disconnect"))
                {
                    gameInstanceManagerNetworked.Disconnect();
                }
            }
            if (started == "Server")
            {
                if (GUI.Button(new Rect(Screen.width - 200, 10, 190, 30), "Start Matchmaking"))
                {
                    //clientNetworked.RequestMatch();
                    DebugLog.Log("Start matchmaking");
                    MatchNetworking match = lobbyNetworked.StartMatchmakingSession();
                    DebugLog.Log(match.ToString());

                }
            }
        }
    }

    private void GIMSendAvailableGameInstances()
    {
        DebugLog.Log("Test: GIMSendAvailableGameInstances()");
    }

    private void StartServer(string lobbyIpAddress, ushort lobbyPort, ushort gicPort)
    {
        
        gicNetworked = new GameInstanceClusterNetworked();
        gicNetworked.Connect(gicPort, Networking.TransportationProtocolType.TCP);
        lobbyNetworked = new LobbyNetworked();
        lobbyNetworked.StartListener(lobbyIpAddress, lobbyPort, Networking.TransportationProtocolType.TCP);
        lobbyNetworked.Matchmaking = new Matchmaking();
    }

    private void StartGameInstanceManager(string lobbyIpAddress, ushort gicPort, ushort gimPort)
    {
        gameInstanceManagerNetworked = new GameInstanceManagerNetworked();
        gameInstanceManagerNetworked.Connect(lobbyIpAddress, gicPort, Networking.TransportationProtocolType.TCP);
        gameInstanceManagerNetworked.ConnectHost(gimPort, Networking.TransportationProtocolType.TCP);
    }

    private void StartGameInstance(string lobbyIpAddress, ushort port)
    {
        gameInstanceNetworked = new GameInstanceNetworked();
        gameInstanceNetworked.Connect(lobbyIpAddress, port, Networking.TransportationProtocolType.TCP);
    }

    private void StartClient(string lobbyIpAddress, ushort lobbyPort)
    {
        if (clientNetworked == null)
        {
            clientNetworked = new ClientNetworked();
        }
        clientNetworked.Connect(lobbyIpAddress, lobbyPort, Networking.TransportationProtocolType.TCP);
    }

    [BRPC]
    public void RequestStartMatch(string message)
    {
        DebugLog.Log("got RequestStartMatch " + message);
        DebugLog.Log(string.Format("player {0}", CurrentRPCSender.NetworkId));
        lobbyNetworked.PlayerRequestStartMatch(message.Split(':'));
    }
}
