using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;

public class GameInstanceClusterNetworked {

    public ushort GICPort { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    private NetWorker NetWorker { get; set; }
    private List<NetworkingPlayer> players;
    private List<GameInstanceNetworked> gameInstanceManagers;

    public int NumberGameInstanceManagers
    {
        get
        {
            return this.gameInstanceManagers.Count;
        }
    }

    public int AvailableGameInstanceManagers
    {
        get
        {
            int count = 0;

            foreach (GameInstanceNetworked instance in this.gameInstanceManagers)
            {
                if (!instance.isMatchRunning)
                {
                    count++;
                }
            }
            return count;
        }
    }


    public GameInstanceClusterNetworked()
    {
        this.gameInstanceManagers = new List<GameInstanceNetworked>();
    }

    void ServerConnected()
    {
        DebugLog.Log("Server: GI Cluster Connected");
    }

    void ServerDisconnected()
    {
        DebugLog.Log("Server: GI Cluster Disconnected");
    }

    void GIMConnected(NetworkingPlayer player)
    {
        DebugLog.Log("Server: GI Manager Added");
        //GameInstanceNetworked gin = new GameInstanceNetworked();
        //gin.NetworkingPlayer = player;

        //this.gameInstances.Add(gin);
        //DebugLog.Log(string.Format("{0} Game Instances Registered", this.gameInstances.Count));
    }

    private GameInstanceNetworked FindGameInstanceFromNetworkingPlayer(NetworkingPlayer player)
    {
        //foreach (GameInstanceNetworked gin in this.gameInstances)
        //{
        //    if (gin.NetworkingPlayer == player)
        //        return gin;
        //}
        return null;
    }

    void GIMDisconnected(NetworkingPlayer player)
    {
        DebugLog.Log("Server: GI Manger Removed");
        //GameInstanceNetworked gin = FindGameInstanceFromNetworkingPlayer(player);
        //if (this.gameInstances.Contains(gin))
        //{
        //    DebugLog.Log("gi disconnected");
        //    this.gameInstances.Remove(gin);
        //    DebugLog.Log(string.Format("{0} game instances registered", this.gameInstances.Count));
        //}
        //else
        //{
        //    //debuglog.log("duplicate player disconnect");
        //}
    }
    public void Connect(ushort gicPort, Networking.TransportationProtocolType protocolType)
    {
        this.GICPort = gicPort;
        this.ProtocolType = protocolType;

        // Networking.InitializeFirewallCheck(port);

        DebugLog.Log(string.Format("Server: Starting GI Cluster {0}",this.GICPort));
        this.NetWorker = Networking.Host(this.GICPort, this.ProtocolType, 100);
        Networking.Sockets[this.GICPort].connected += ServerConnected;
        Networking.Sockets[this.GICPort].disconnected += ServerDisconnected;
        Networking.Sockets[this.GICPort].playerConnected += GIMConnected;
        Networking.Sockets[this.GICPort].playerDisconnected += GIMDisconnected;
    }

    public void Disconnect()
    {
        Networking.Disconnect(this.NetWorker);
    }
}
