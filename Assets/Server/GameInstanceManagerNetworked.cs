using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;

public class GameInstanceManagerNetworked {

    public ushort Port { get; set; }
    public Networking.TransportationProtocolType ProtocolType { get; set; }
    private NetWorker NetWorker { get; set; }

    public GameInstanceManagerNetworked()
    {
    }

    public void Connect(ushort port, Networking.TransportationProtocolType protocolType)
    {
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
