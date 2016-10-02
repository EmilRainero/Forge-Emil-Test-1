using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;

public class GameInstanceNetworked {

    public string Host { get; set; }
    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    public NetWorker NetWorker { get; set; }
    private bool matchRunning = false;
    public bool isMatchRunning
    {
        get
        {
            return matchRunning;
        }
    }

    public Match Match { get; set; }
    public NetworkingPlayer NetworkingPlayer { get; set; }

    public void StartMatch(Match match)
    {
        this.Match = match;
        matchRunning = true;
    }

    public void EndMatch()
    {
        matchRunning = false;
    }

    public GameInstanceNetworked()
    {
    }

    void ClientConnected()
    {
        DebugLog.Log("Game Instance Connected");
    }

    void ClientDisconnected()
    {
        DebugLog.Log("Game Instance Disconnected");
    }

    public void Connect(string host, ushort port, Networking.TransportationProtocolType protocolType)
    {
        this.Host = host;
        this.Port = port;
        this.ProtocolType = protocolType;

        DebugLog.Log(string.Format("Game Instance try to connect to {0}:{1}", host, port));
        this.NetWorker = Networking.Connect(this.Host, this.Port, this.ProtocolType);
        Networking.Sockets[this.Port].connected += ClientConnected;
        Networking.Sockets[this.Port].disconnected += ClientDisconnected;
    }

    public void Disconnect()
    {
        Networking.Disconnect(this.NetWorker);
    }
}
