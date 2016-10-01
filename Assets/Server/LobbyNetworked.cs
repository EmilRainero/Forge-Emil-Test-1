using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;

public class LobbyNetworked
{
    public string Host { get; set; }
    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    private NetWorker NetWorker { get; set; }

   

    private List<NetworkingPlayer> players;
    private int maxPlayers;

    public LobbyNetworked(int maxPlayers = int.MaxValue)
    {
        this.maxPlayers = maxPlayers;
        this.players = new List<NetworkingPlayer>();
    }

    public int NumberPlayers
    {
        get
        {
            return players.Count;
        }
    }
    public void AddPlayer(NetworkingPlayer player)
    {
        if (this.NumberPlayers >= this.maxPlayers)
            throw new ArgumentException("max number of players");
        this.players.Add(player);
    }

    public void RemovePlayer(NetworkingPlayer player)
    {
        if (players.Contains(player))
        {
            players.Remove(player);
        }
        else
        {
            throw new ArgumentException("not found");
        }
    }

    public void RemoveAllPlayers()
    {
        players.Clear();
    }

    void ServerConnected()
    {
        DebugLog.Log("Connected");
    }

    void ServerDisconnected()
    {
        DebugLog.Log("Disconnected");
    }

    void PlayerConnected(NetworkingPlayer player)
    {
        DebugLog.Log("PlayerConnected-- " + player.NetworkId);
        this.players.Add(player);
    }

    void PlayerDisconnected(NetworkingPlayer player)
    {
        Debug.Log("CALLED PlayerDisconnected " + this.players.Count);
        //DebugLog.Log("PlayerDisconnected-- " + player.NetworkId);
        this.players.Remove(player);
    }

    public void Connect(string host, ushort port, Networking.TransportationProtocolType protocolType)
    {
        this.Host = host;
        this.Port = port;
        this.ProtocolType = protocolType;

        //Networking.InitializeFirewallCheck(port);
        this.NetWorker = Networking.Host(this.Port, this.ProtocolType, 100);
        Networking.Sockets[this.Port].connected += ServerConnected;
        Networking.Sockets[this.Port].disconnected += ServerDisconnected;
        Networking.Sockets[this.Port].playerConnected += PlayerConnected;
        Networking.Sockets[this.Port].playerDisconnected += PlayerDisconnected;
    }

    public void Disconnect()
    {
        Networking.Disconnect(this.NetWorker);
    }
}
