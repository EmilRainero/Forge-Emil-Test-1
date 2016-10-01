using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;

public class GameInstanceNetworked : NetworkedMonoBehavior {

    public GameInstance GameInstance { get; set; }
    public string Host { get; set; }
    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    public NetWorker NetWorker { get; set; }

    public GameInstanceNetworked(GameInstance gameInstance)
    {
        this.GameInstance = gameInstance;
    }

    public void Connect(string host, ushort port, Networking.TransportationProtocolType protocolType)
    {
        this.Host = host;
        this.Port = port;
        this.ProtocolType = protocolType;

        Networking.InitializeFirewallCheck(port);

        this.NetWorker = Networking.Host(this.Port, this.ProtocolType, 100);
    }

    public void Disconnect()
    {
        Networking.Disconnect(this.NetWorker);
    }
}
