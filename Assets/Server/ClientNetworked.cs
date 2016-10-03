using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using System;
using System.Collections.Generic;

namespace ScalableServer
{

    public class ClientNetworked : SimpleNetworkedMonoBehavior
    {
        public enum ClientState
        {
            INITIAL,
            TRYING_TO_CONNECT,
            CONNECTED_TO_LOBBY,
            REQUESTED_MATCH,
            IN_MATCH
        }
        private ClientState _State;
        public ClientState State
        {
            get {
                return this._State;
            }
        }

        public string Host { get; set; }
        public ushort Port { get; set; }
        public Networking.TransportationProtocolType ProtocolType { get; set; }
        public NetWorker NetWorker { get; set; }

        public ClientNetworked()
        {
            this._State = ClientState.INITIAL;
        }

        void ClientConnected()
        {
            DebugLog.Log("Connected");
            this._State = ClientState.CONNECTED_TO_LOBBY;
        }

        void ClientDisconnected()
        {
            DebugLog.Log("Disconnected");
            this._State = ClientState.INITIAL;
        }

        public void Connect(string host, ushort port, Networking.TransportationProtocolType protocolType)
        {
            this.Host = host;
            this.Port = port;
            this.ProtocolType = protocolType;

            this._State = ClientState.TRYING_TO_CONNECT;
            DebugLog.Log(string.Format("try to connect to {0}:{1}", host, port));
            this.NetWorker = Networking.Connect(this.Host, this.Port, this.ProtocolType);
            Networking.Sockets[this.Port].connected += ClientConnected;
            Networking.Sockets[this.Port].disconnected += ClientDisconnected;
        }

        public void DisconnectClient()
        {
            Networking.Disconnect(this.NetWorker);
        }

        internal void RequestMatch()
        {
            DebugLog.Log("Request start match");
            this._State = ClientState.REQUESTED_MATCH;
            RPC("RequestStartMatch", NetworkReceivers.Server, "hi there");
        }
    }
}