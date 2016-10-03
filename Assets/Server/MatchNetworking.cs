using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using BeardedManStudios.Network;

namespace ScalableServer
{
    public class MatchNetworking
    {
        public enum MatchType
        {
            Death,
            CaptureTheFlag
        }

        private List<List<NetworkingPlayer>> Teams;

        private MatchType matchType;

        public MatchType Type {
            get 
            {
                return this.matchType;
            }
        }

        public MatchNetworking(int numberTeams = 2, MatchType matchType = MatchType.Death)
        {
            this.Teams = new List<List<NetworkingPlayer>>();
            this.matchType = matchType;
            for (int i=0; i<numberTeams; i++)
            {
                this.Teams.Add(new List<NetworkingPlayer>());
            }
        }

        public int NumberTeams {
            get
            {
                return this.Teams.Count;
            }
        }

        public void AddPlayerToTeam(NetworkingPlayer player, int team)
        {
            this.Teams[team].Add(player);
        }

        public List<NetworkingPlayer> GetTeam(int team)
        {
            return this.Teams[team];
        }
    }
}
