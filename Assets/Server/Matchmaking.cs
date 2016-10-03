using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using BeardedManStudios.Network;

namespace ScalableServer
{
    public class Matchmaking
    {
        private List<NetworkingPlayer> availablePlayers;

        public Matchmaking()
        {
            this.availablePlayers = new List<NetworkingPlayer>();
        }

        public int NumberAvaialablePlayers
        {
            get
            {
                return this.availablePlayers.Count;
            }
        }

        public void AddAvailablePlayer(NetworkingPlayer player)
        {
            this.availablePlayers.Add(player);
        }

        public void RemoveAvailablePlayer(NetworkingPlayer player, bool throwIfNotContained = false)
        {
            if (this.availablePlayers.Contains(player))
            {
                this.availablePlayers.Remove(player);
            }
            else
            {
                if (throwIfNotContained)
                {
                    throw new ArgumentException("not found");
                }
            }
        }

        public MatchNetworking MakeAMatch(int playersPerSide = 1, int numberTeams = 2, Predicate<NetworkingPlayer> includePredicate = null)
        {
            List<NetworkingPlayer> players = this.availablePlayers;
            if (includePredicate != null)
            {
                players = players.FindAll(includePredicate);
            }
            if (players.Count < playersPerSide * numberTeams)
                throw new ArgumentException("Not enough players");

            MatchNetworking match = new MatchNetworking(numberTeams);

            for (int i = 0; i < playersPerSide * numberTeams; i += numberTeams)
            {
                for (int j = 0; j < numberTeams; j++)
                {
                    match.AddPlayerToTeam(players[i + j], j);
                }
            }
            return match;
        }

        public Match MakeAMatch(List<Player> players, int playersPerSide = 1, int numberTeams = 2, Predicate<Player> includePredicate = null)
        {
            if (includePredicate != null)
            {
                players = players.FindAll(includePredicate);
            }
            if (players.Count < playersPerSide * numberTeams)
                throw new ArgumentException("Not enough players");

            Match match = new Match(numberTeams);

            for (int i=0; i<playersPerSide*numberTeams; i+=numberTeams)
            {
                for (int j=0; j<numberTeams; j++)
                {
                    match.AddPlayerToTeam(players[i+j], j);
                }
            }
            return match;
        }
    }
}
