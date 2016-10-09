using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;
using System;
using System.Collections.Generic;


namespace ScalableServer
{

    public class LobbyNetworked : SimpleNetworkedMonoBehavior
    {
        public string Host { get; set; }
        public ushort Port { get; set; }
        public Networking.TransportationProtocolType ProtocolType { get; set; }
        private NetWorker NetWorker { get; set; }
        public Matchmaking Matchmaking { get; set; }

        private List<NetworkingPlayer> players;
        public int MaxPlayers = int.MaxValue;

        private IServerNetworkCalls ServerNetworkCalls;
        private NetWorker worker;

        public LobbyNetworked(IServerNetworkCalls serverNetworkCalls)
        {
            this.ServerNetworkCalls = serverNetworkCalls;
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
            if (this.NumberPlayers >= this.MaxPlayers)
                throw new ArgumentException("max number of players");
            this.players.Add(player);
        }

        public void RemoveAllPlayers()
        {
            players.Clear();
        }

        void ServerConnected()
        {
            DebugLog.Log("Server: Lobby Connected");
        }

        void ServerDisconnected()
        {
            DebugLog.Log("Server: Lobby Diconnected");
        }

        void PlayerConnected(NetworkingPlayer player)
        {
            DebugLog.Log("PlayerConnected");
            this.players.Add(player);
        }

        void PlayerDisconnected(NetworkingPlayer player)
        {
            //DebugLog.Log("PlayerDisconnected-- " + player.NetworkId);
            RemovePlayer(player, false);
            this.Matchmaking.RemoveAvailablePlayer(player, false);
        }

        public void RemovePlayer(NetworkingPlayer player, bool throwIfNotContained = false)
        {
            if (this.players.Contains(player))
            {
                this.players.Remove(player);
            }
            else
            {
                if (throwIfNotContained)
                {
                    throw new ArgumentException("not found");
                }
            }
        }

        public void StartListener(string host, ushort port, Networking.TransportationProtocolType protocolType)
        {
            this.Host = host;
            this.Port = port;
            this.ProtocolType = protocolType;

            //Networking.InitializeFirewallCheck(port);
            DebugLog.Log(string.Format("Server: Starting Lobby {0}", this.Port));
            this.NetWorker = Networking.Host(this.Port, this.ProtocolType, 100);
            Networking.Sockets[this.Port].connected += ServerConnected;
            Networking.Sockets[this.Port].disconnected += ServerDisconnected;
            Networking.Sockets[this.Port].playerConnected += PlayerConnected;
            Networking.Sockets[this.Port].playerDisconnected += PlayerDisconnected;
        }

        public NetWorker StartServer(ushort port, Networking.TransportationProtocolType protocol, int maxConnections = int.MaxValue)
        {
            worker = this.ServerNetworkCalls.NetworkHost(port, protocol, maxConnections, ServerConnected, ServerDisconnected, PlayerConnected, PlayerDisconnected);
            return worker;
        }

        public void StopListener()
        {
            Networking.Disconnect(this.NetWorker);
        }

        public void PlayerRequestStartMatch(string[] args)
        {
            ulong networkId = ulong.Parse(args[1]);

            NetworkingPlayer player = FindPlayer(networkId);
            if (player != null)
            {
                DebugLog.Log("Found player");
                this.Matchmaking.AddAvailablePlayer(player);
                DebugLog.Log(string.Format("Matchmaking: {0} players available", this.Matchmaking.NumberAvaialablePlayers));
            }
            else
            {
                DebugLog.Log(string.Format("Could not find player {0}", networkId));
            }
        }

        private NetworkingPlayer FindPlayer(ulong networkId)
        {
            foreach (NetworkingPlayer player in this.players)
            {
                if (player.NetworkId == networkId)
                {
                    return player;
                }
            }
            return null;
        }

        public MatchNetworking StartMatchmakingSession()
        {
            DebugLog.Log(string.Format("StartMatchmakingSession()  availablePlayes: {0}", this.Matchmaking.NumberAvaialablePlayers));
            if (this.Matchmaking.NumberAvaialablePlayers >= 2)
            {
                DebugLog.Log("enough players");
            }
            MatchNetworking match = this.Matchmaking.MakeAMatch();

            return match;
        }
    }

}