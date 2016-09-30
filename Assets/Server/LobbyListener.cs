using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;

public class LobbyListener : NetworkedMonoBehavior
{
    private Lobby lobby;
    public int Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }


    public LobbyListener(Lobby lobby)
    {
        this.lobby = lobby;
    }

    public void Connect(int port, Networking.TransportationProtocolType protocolType)
    {
        this.Port = port;
        this.ProtocolType = protocolType;
    }

    public void Disconnect()
    {

    }
}
