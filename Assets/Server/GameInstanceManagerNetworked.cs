using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;

public class GameInstanceManagerNetworked {

    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    private NetWorker NetWorker { get; set; }
    private List<NetworkingPlayer> gameInstances;

    public GameInstanceManagerNetworked()
    {
        this.gameInstances = new List<NetworkingPlayer>();
    }

    void ServerConnected()
    {
        DebugLog.Log("Server Connected");
    }

    void ServerDisconnected()
    {
        DebugLog.Log("Server Disconnected");
    }

    void GIConnected(NetworkingPlayer player)
    {
        DebugLog.Log("GI Connected");
        this.gameInstances.Add(player);
        DebugLog.Log(string.Format("{0} Game Instances Registered", this.gameInstances.Count));
    }

    void GIDisconnected(NetworkingPlayer player)
    {
        if (this.gameInstances.Contains(player))
        {
            DebugLog.Log("GI Disconnected");
            this.gameInstances.Remove(player);
            DebugLog.Log(string.Format("{0} Game Instances Registered", this.gameInstances.Count));
        }
        else
        {
            //DebugLog.Log("Duplicate player disconnect");
        }
    }
    public void Connect(ushort port, Networking.TransportationProtocolType protocolType)
    {
        this.Port = port;
        this.ProtocolType = protocolType;

       // Networking.InitializeFirewallCheck(port);

        this.NetWorker = Networking.Host(this.Port, this.ProtocolType, 100);
        Networking.Sockets[this.Port].connected += ServerConnected;
        Networking.Sockets[this.Port].disconnected += ServerDisconnected;
        Networking.Sockets[this.Port].playerConnected += GIConnected;
        Networking.Sockets[this.Port].playerDisconnected += GIDisconnected;
    }

    public void Disconnect()
    {
        Networking.Disconnect(this.NetWorker);
    }
}
