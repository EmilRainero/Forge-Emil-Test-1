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
    public Networking.TransportationProtocolType PROTOCOL_TYPE = Networking.TransportationProtocolType.UDP;
    public int PLAYER_COUNT = 31;
    private NetWorker worker = null;
    private bool isServer = true;
    public Text textLog;

    private void AddToLog(string message)
    {
        textLog.text = textLog.text + message + "\n";
    }

    override protected void OnApplicationQuit()
    {
        AddToLog("Quit");
    }

    void Start()
    {
        players = new List<NetworkingPlayer>();
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
            StartClient();
            isServer = false;
        }
        if (worker != null && !isServer && GUI.Button(new Rect(Screen.width - 200, 10, 190, 50), "Stop Client"))
        {
            StopClient();
        }
        if (worker != null && isServer && GUI.Button(new Rect(Screen.width - 200, 10, 190, 50), "Run Matchmaking"))
        {
            RunMatchmaking();
        }
    }

    private ulong matchmakingUniqueID = 0;

    private void RunMatchmaking()
    {
        AddToLog(string.Format("RunMatchmaking: {0} players waiting for match", players.Count));

        if (players.Count < 2)
            return;

        matchmakingUniqueID++;
        ulong timeInSeconds = (ulong) Time.realtimeSinceStartup;

        int maxPlayers = 2;
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
            AddToLog(string.Format("\tplayer {0}", playersForMatch[i].NetworkId));
        }
        for (int i = 0; i < playersForMatch.Count; i++)
        {
            players.Remove(playersForMatch[i]);
        }
        AddToLog(string.Format("RunMatchmaking: {0} players still waiting for match", players.Count));

        for (int i = 0; i < playersForMatch.Count; i++)
        {
            string message = string.Format("MM {0}: Game:{1}  Port: {2}", matchmakingUniqueID, i % 2, 12345);
            AuthoritativeRPC("MatchMakingRPC", worker, playersForMatch[i], true, message);
        }
    }

    [BRPC]
    private void MatchMakingRPC(string message)
    {
        if (!isServer)
        {
            AddToLog("MM Response:" + message);
            StartCoroutine(Client_RunAMatch(2f));
        }
    }

    IEnumerator Client_RunAMatch(float waitTime)
    {
        AddToLog("Client_RunAMatch Start");
        yield return new WaitForSeconds(waitTime);
        AddToLog("Client_RunAMatch End");
    }

    void StartServer()
    {
        AddToLog("Starting Server");
        worker = Networking.Host(PORT, PROTOCOL_TYPE, PLAYER_COUNT);
        Networking.Sockets[PORT].connected += delegate ()
        {
            AddToLog("Connected");
        };
        Networking.Sockets[PORT].disconnected += delegate ()
        {
            AddToLog("Disconnected");
        };
        Networking.Sockets[PORT].playerConnected += delegate (NetworkingPlayer player)
        {
            AddToLog("PlayerConnected");
            players.Add(player);
            AddToLog(string.Format("{0} players", players.Count));
        };
        Networking.Sockets[PORT].playerDisconnected += delegate (NetworkingPlayer player)
        {
            AddToLog("PlayerDisconnected");
            players.Remove(player);
            AddToLog(string.Format("{0} players", players.Count));
        };
    }

    void StartClient()
    {
        AddToLog("Start Client");

        worker = Networking.Connect(HOST, PORT, PROTOCOL_TYPE, true);
        Networking.Sockets[PORT].connected += delegate ()
        {
            AddToLog("Connected");
        };
        Networking.Sockets[PORT].disconnected += delegate ()
        {
            AddToLog("Disconnected");
        };
        Networking.Sockets[PORT].serverDisconnected += delegate (string reason)
        {
            AddToLog("The server has disconnected you because of: " + reason);
        };
    }

    void StopClient()
    {
        AddToLog("Stop Client");
        if (worker != null)
        {
            Networking.Disconnect(worker);
            worker = null;
        }
    }
}

