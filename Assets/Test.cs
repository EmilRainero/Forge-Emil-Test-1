using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using System.Collections.Generic;
using UnityEngine.UI;

public class Test : NetworkedMonoBehavior
{
    private List<NetworkingPlayer> players;
    public string HOST = "127.0.0.1";
    public ushort PORT = 15937;
    public Networking.TransportationProtocolType PROTOCOL_TYPE = Networking.TransportationProtocolType.TCP;
    public int PLAYER_COUNT = 31;
    private NetWorker worker = null;
    private bool isServer = true;
    public Text textLog;
    public ushort matchStartingPort = 15938;
    public ushort matchEndingPort = 15980;
    private Dictionary<ushort, ushort> matchPortsInUse;
    private ushort currentPort;
    public GameObject player;

    private void AddToLog(string message)
    {
        textLog.text = textLog.text + message + "\n";
        Debug.Log(message);
    }

    override protected void OnApplicationQuit()
    {
        AddToLog("Quit");
    }

    void Start()
    {
        players = new List<NetworkingPlayer>();
        matchPortsInUse = new Dictionary<ushort, ushort>();
    }

    IEnumerator RunMatchmakingCoroutine(float waitTime)
    {
        while (true)
        {
            yield return new WaitForSeconds(waitTime);
            RunMatchmaking();
        }
    }

    void OnGUI()
    {
        if (worker == null && GUI.Button(new Rect(Screen.width - 200, 10, 190, 50), "Start Server"))
        {
            StartServer();
            isServer = true;
            StartCoroutine(RunMatchmakingCoroutine(10F));
        }
        if (worker == null && GUI.Button(new Rect(Screen.width - 200, 70, 190, 50), "Start Client"))
        {
            StartClient(PORT);
            isServer = false;
        }
        if (worker != null && !isServer && GUI.Button(new Rect(Screen.width - 200, 10, 190, 50), "Stop Client"))
        {
            StopClient(PORT);
        }
        if (worker != null && isServer && GUI.Button(new Rect(Screen.width - 200, 10, 190, 50), "Run Matchmaking"))
        {
            RunMatchmaking();
        }
    }

    private ulong matchmakingUniqueID = 0;

    private void RunMatchmaking()
    {

        if (players.Count < 1)
            return;

        AddToLog(string.Format("RunMatchmaking: {0} players waiting for match", players.Count));

        int maxPlayers = 3;
        List<NetworkingPlayer> playersForMatch = new List<NetworkingPlayer>();
        for (int i = 0; i < players.Count; i++)
        {
            if (true) // ok to add this player to the match?
            {
                playersForMatch.Add(players[i]);
                if (playersForMatch.Count >= maxPlayers)
                    break;
            }
        }

        AddToLog(string.Format("RunMatchmaking: made a match with {0} players", playersForMatch.Count));
        for (int i = 0; i < playersForMatch.Count; i++)
        {
            players.Remove(playersForMatch[i]);
        }
        AddToLog(string.Format("RunMatchmaking: {0} players still waiting for match", players.Count));

        StartAMatch(playersForMatch);
    }


    private ushort assignMatchPort()
    {
        ushort port;

        for (port= matchStartingPort; port <= matchEndingPort; port+=2)
        {
            if (!matchPortsInUse.ContainsKey(port))
            {
                matchPortsInUse.Add(port, port);
                return port;
            }
        }
        return 0; // no ports available
    }

    private void releaseMatchPort(ushort port)
    {
        if (matchPortsInUse.ContainsKey(port))
        {
            matchPortsInUse.Remove(port);
        }
    }

    private void StartAMatch(List<NetworkingPlayer> playersForMatch)
    {
        ushort assignedPort = assignMatchPort();
        if (assignedPort != 0)
        {
            matchmakingUniqueID++;
            StartMatchServer(assignedPort, playersForMatch);
        }
        else
        {
            // no more ports available for matches
            AddToLog("No more ports available for matches. Players have to wait.");
            foreach (NetworkingPlayer player in playersForMatch)
            {
                players.Add(player);
            }
        }
    }

    [BRPC]
    private void StartGameRPC(string message)
    {
        if (!isServer)
        {
            AddToLog("StartGameRPC");
            AddToLog(string.Format("StartGameRPC   worker: {0}  OwningNetWorker: {1}", worker.Port, OwningNetWorker.Port));
            //SimpleNetworkedMonoBehavior snmb = player.GetComponent<SimpleNetworkedMonoBehavior>();
            //AddToLog(string.Format("player port: {0}", snmb.OwningNetWorker.Port));

            Networking.Instantiate(player, NetworkReceivers.AllBuffered);
        }
    }

    [BRPC]
    private void EndGameRPC(string message)
    {
        if (!isServer)
        {
            AddToLog("EndGameRPC");
        }
    }

    [BRPC]
    private void MatchMakingRPC(string message)
    {
        if (!isServer)
        {
            AddToLog("MM Response:" + message);
            string[] args = message.Split(':');
            AddToLog(string.Format("client got port {0}", args[3]));
            StopClient(currentPort);
            currentPort = ushort.Parse(args[3]);
            StartClient(currentPort);
            //StartCoroutine(Client_RunAMatch(2f));
        }
    }

    IEnumerator Client_RunAMatch(float waitTime)
    {
        AddToLog("Client_RunAMatch Start");
        yield return new WaitForSeconds(waitTime);
        AddToLog("Client_RunAMatch End");
    }

    void ServerConnected()
    {
        AddToLog("Connected");
    }

    void ServerDisconnected()
    {
        AddToLog("Disconnected");
    }
    
    void PlayerConnected(NetworkingPlayer player)
    {
        AddToLog("PlayerConnected");
        players.Add(player);
        AddToLog(string.Format("{0} players", players.Count));
    }

    void PlayerDisconnected(NetworkingPlayer player)
    {
        AddToLog("PlayerDisconnected");
        players.Remove(player);
        AddToLog(string.Format("{0} players", players.Count));
    }

    void StartServer()
    {
        AddToLog("Starting Server");
        worker = Networking.Host(PORT, PROTOCOL_TYPE, PLAYER_COUNT);
        Networking.Sockets[PORT].connected += ServerConnected;
        Networking.Sockets[PORT].disconnected += ServerDisconnected;
        Networking.Sockets[PORT].playerConnected += PlayerConnected;
        Networking.Sockets[PORT].playerDisconnected += PlayerDisconnected;
    }

    void ErrorCallback(System.Exception exception)
    {
        AddToLog("Error: " + exception.Message);
    }

    void StartMatchServer(ushort port, List<NetworkingPlayer> playersForMatch)
    {
        AddToLog("Starting Match Server");

        string message = string.Format("MM:{0}:Port:{1}", matchmakingUniqueID, port);
        AddToLog("Starting match: " + message);

        for (int i = 0; i < playersForMatch.Count; i++)
        {
            AddToLog(string.Format("\tplayer {0}", playersForMatch[i].NetworkId));
        }

        AddToLog(string.Format("Starting Match Server on port {0}", port));

        worker = Networking.Host(port, PROTOCOL_TYPE, PLAYER_COUNT, false, null, false, true, false, ErrorCallback);
        Debug.Log(worker);
        Networking.Sockets[port].connected += delegate ()
        {
            AddToLog("Match Server Connected");

            for (int i = 0; i < playersForMatch.Count; i++)
            {
                AuthoritativeRPC("MatchMakingRPC", worker, playersForMatch[i], false, message);
            }

            StartCoroutine(BroadcastMatchStartCoroutine(port, 3F));
        };
        Networking.Sockets[port].disconnected += delegate ()
        {
            AddToLog("Match Server Disconnected");
        };
        Networking.Sockets[port].playerConnected += delegate (NetworkingPlayer player)
        {
            AddToLog(string.Format("Match Player Connected on port {0}", port));
        };
        Networking.Sockets[port].playerDisconnected += delegate (NetworkingPlayer player)
        {
            AddToLog(string.Format("Match Player Disconnected on port {0}", port));
        };
    }

    IEnumerator BroadcastMatchStartCoroutine(ushort port, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        AddToLog(string.Format("broadcastMatchStart port: {0}", port));
        URPC("StartGameRPC", Networking.Sockets[port], NetworkReceivers.All, "");

        yield return new WaitForSeconds(waitTime);
        URPC("EndGameRPC", Networking.Sockets[port], NetworkReceivers.All, "");
    }

    void StopServer()
    {
        AddToLog("Stopping Server");
    }

    private void ClientConnected()
    {
        AddToLog("Connected");
        AddToLog(string.Format("ClientConnected   worker: {0}", worker.Port));

        //Networking.Instantiate(player, NetworkReceivers.AllBuffered);
    }

    private void ClientDisconnected()
    {
        AddToLog("Disconnected");
    }

    private void ServerDisconnected(string reason)
    {
        AddToLog("The server has disconnected you because of: " + reason);
    }

    void StartClient(ushort port)
    {
        AddToLog("Start Client");

        worker = Networking.Connect(HOST, port, PROTOCOL_TYPE, true);
        AddToLog(string.Format("StartClient   port: {0}", port));
        Networking.Sockets[port].connected += ClientConnected;
        Networking.Sockets[port].disconnected += ClientDisconnected;
        Networking.Sockets[port].serverDisconnected += ServerDisconnected;
        currentPort = port;
    }

    void StopClient(ushort port)
    {
        AddToLog("Stop Client");
        if (worker != null)
        {
            Networking.Sockets[port].connected -= ClientConnected;
            Networking.Sockets[port].disconnected -= ClientDisconnected;
            Networking.Sockets[port].serverDisconnected -= ServerDisconnected;

            Networking.Disconnect(worker);
            NetworkDisconnect();
            //Networking.NetworkingReset();
            worker = null;
        }
    }
}

