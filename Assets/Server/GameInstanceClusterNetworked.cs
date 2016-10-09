using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;

namespace ScalableServer
{

    public class GameInstanceClusterNetworked
    {

        public ushort GICPort { get; set; }
        public Networking.TransportationProtocolType ProtocolType { get; set; }
        private NetWorker NetWorker { get; set; }
        private List<NetworkingPlayer> players;
        private List<GameInstanceManagerNetworked> gameInstanceManagers;

        public int NumberGameInstanceManagers
        {
            get
            {
                return this.gameInstanceManagers.Count;
            }
        }

        public int AvailableGameInstances
        {
            get
            {
                int count = 0;

                //foreach (GameInstanceManagerNetworked gim in this.gameInstanceManagers)
                //{
                //    count += gim.AvailableGameInstances;
                //}
                return count;
            }
        }

        public int AvailableGameInstanceManagers
        {
            get
            {
                int count = 0;

                //foreach (GameInstanceManagerNetworked instance in this.gameInstanceManagers)
                //{
                //}
                return count;
            }
        }


        public GameInstanceClusterNetworked()
        {
            this.gameInstanceManagers = new List<GameInstanceManagerNetworked>();
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

            GameInstanceManagerNetworked gim = new GameInstanceManagerNetworked();
            gim.NetworkingPlayer = player;
            this.gameInstanceManagers.Add(gim);
        }

        private GameInstanceManagerNetworked FindGameInstanceManagerFromNetworkingPlayer(NetworkingPlayer player)
        {
            foreach (GameInstanceManagerNetworked gim in this.gameInstanceManagers)
            {
                if (gim.NetworkingPlayer == player)
                    return gim;
            }
            return null;
        }

        void GIMDisconnected(NetworkingPlayer player)
        {
            GameInstanceManagerNetworked gim = FindGameInstanceManagerFromNetworkingPlayer(player);
            if (gim != null)
            {
                DebugLog.Log("Server: GI Manger Removed");
                this.gameInstanceManagers.Remove(gim);
            }
        }

        public void Connect(ushort gicPort, Networking.TransportationProtocolType protocolType)
        {
            this.GICPort = gicPort;
            this.ProtocolType = protocolType;

            // Networking.InitializeFirewallCheck(port);

            DebugLog.Log(string.Format("Server: Starting GI Cluster {0}", this.GICPort));
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
}
