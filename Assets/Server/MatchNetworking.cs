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

        public override string ToString()
        {
            string result = string.Empty;
            result += string.Format("Match:{0}\r\n", this.matchType.ToString());
            for (int i=0; i<this.NumberTeams; i++)
            {
                result += string.Format("  Team:{0}\r\n", i);
                foreach (NetworkingPlayer player in this.Teams[i])
                {
                    result += string.Format("    {0}\r\n", player.NetworkId);
                }
            }

            return result;
        }
    }
}
