using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;

public class GameInstanceManagerNetworked {
    public NetworkingPlayer NetworkingPlayer { get; set; }
    public string ServerHost { get; set; }
    public ushort GICPort { get; set; }
    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    private NetWorker NetWorker { get; set; }
    private NetWorker GICNetWorker { get; set; }
    private List<NetworkingPlayer> players;
    private List<GameInstanceNetworked> gameInstances;

    public int NumberGameInstances
    {
        get
        {
            return this.gameInstances.Count;
        }
    }

    public int AvailableGameInstances
    {
        get
        {
            int count = 0;

            foreach (GameInstanceNetworked instance in this.gameInstances)
            {
                if (!instance.isMatchRunning)
                {
                    count++;
                }
            }
            return count;
        }
    }


    public GameInstanceManagerNetworked()
    {
        this.gameInstances = new List<GameInstanceNetworked>();
    }

    void ServerConnected()
    {
        DebugLog.Log("GI Manager Connected");
    }

    void ServerDisconnected()
    {
        DebugLog.Log("GI Manager Disconnected");
    }

    void GIConnected(NetworkingPlayer player)
    {
        GameInstanceNetworked gin = FindGameInstanceFromNetworkingPlayer(player);
        if (gin != null)
        {
            // already added
            return;
        }
        DebugLog.Log("GI Connected");

        gin = new GameInstanceNetworked();
        gin.NetworkingPlayer = player;

        this.gameInstances.Add(gin);
        SendAvailableGameInstances();
    }

    private void SendAvailableGameInstances()
    {
        DebugLog.Log(string.Format("{0} game instances registered", this.gameInstances.Count));
    }

    private GameInstanceNetworked FindGameInstanceFromNetworkingPlayer(NetworkingPlayer player)
    {
        foreach (GameInstanceNetworked gin in this.gameInstances)
        {
            if (gin.NetworkingPlayer == player)
                return gin;
        }
        return null;
    }

    void GIDisconnected(NetworkingPlayer player)
    {
        GameInstanceNetworked gin = FindGameInstanceFromNetworkingPlayer(player);
        if (this.gameInstances.Contains(gin))
        {
            DebugLog.Log("GI Disconnected");
            this.gameInstances.Remove(gin);
            SendAvailableGameInstances();
        }
        else
        {
            //debuglog.log("duplicate player disconnect");
        }
    }
    public void ConnectHost(ushort port, Networking.TransportationProtocolType protocolType)
    {
        this.Port = port;
        this.ProtocolType = protocolType;

        // Networking.InitializeFirewallCheck(port);

        DebugLog.Log(string.Format("Gi Manager: Starting {0}", this.Port));
        this.NetWorker = Networking.Host(this.Port, this.ProtocolType, 100);
        Networking.Sockets[this.Port].connected += ServerConnected;
        Networking.Sockets[this.Port].disconnected += ServerDisconnected;
        Networking.Sockets[this.Port].playerConnected += GIConnected;
        Networking.Sockets[this.Port].playerDisconnected += GIDisconnected;
    }

    public void Disconnect()
    {
        DebugLog.Log("Disconnect from GIC");
        Networking.Disconnect(this.GICNetWorker);
        //Networking.Disconnect(this.NetWorker);
    }

    void ClientConnected()
    {
        DebugLog.Log("Connected to GIC");
    }

    void ClientDisconnected()
    {
        DebugLog.Log("Disconnected from GIC");
    }

    public void Connect(string lobbyIpAddress, ushort gicPort, Networking.TransportationProtocolType protocolType)
    {
        this.ServerHost = lobbyIpAddress;
        this.GICPort = gicPort;
        this.ProtocolType = protocolType;

        DebugLog.Log(string.Format("try to connect to GI Cluster {0}:{1}", this.ServerHost, this.GICPort));
        this.GICNetWorker = Networking.Connect(this.ServerHost, this.GICPort, this.ProtocolType);
        Networking.Sockets[this.GICPort].connected += ClientConnected;
        Networking.Sockets[this.GICPort].disconnected += ClientDisconnected;
    }

}
