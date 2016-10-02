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
        DebugLog.Log("Server Connected");
    }

    void ServerDisconnected()
    {
        DebugLog.Log("Server Disconnected");
    }

    void GIConnected(NetworkingPlayer player)
    {
        DebugLog.Log("GI Connected");
        GameInstanceNetworked gin = new GameInstanceNetworked();
        gin.NetworkingPlayer = player;

        this.gameInstances.Add(gin);
        DebugLog.Log(string.Format("{0} Game Instances Registered", this.gameInstances.Count));
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
            DebugLog.Log("gi disconnected");
            this.gameInstances.Remove(gin);
            DebugLog.Log(string.Format("{0} game instances registered", this.gameInstances.Count));
        }
        else
        {
            //debuglog.log("duplicate player disconnect");
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
