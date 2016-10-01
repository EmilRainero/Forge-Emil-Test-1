using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using System;
using System.Collections.Generic;

namespace ScalableServer
{

    public class ClientNetworked
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public Networking.TransportationProtocolType ProtocolType { get; set; }
        private NetWorker NetWorker { get; set; }

        public ClientNetworked()
        {
        }

        void ClientConnected()
        {
            DebugLog.Log("Connected");
        }

        void ClientDisconnected()
        {
            DebugLog.Log("Disconnected");
        }

        public void Connect(string host, ushort port, Networking.TransportationProtocolType protocolType)
        {
            this.Host = host;
            this.Port = port;
            this.ProtocolType = protocolType;

            DebugLog.Log(string.Format("try to connect to {0}:{1}", host, port));
            this.NetWorker = Networking.Connect(this.Host, this.Port, this.ProtocolType);
            Networking.Sockets[this.Port].connected += ClientConnected;
            Networking.Sockets[this.Port].disconnected += ClientDisconnected;
        }

        public void Disconnect()
        {
            Networking.Disconnect(this.NetWorker);
        }
    }
}