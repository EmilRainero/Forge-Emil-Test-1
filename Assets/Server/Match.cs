using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ScalableServer
{
    public class Match
    {
        public enum MatchType
        {
            Death,
            CaptureTheFlag
        }

        private List<List<Player>> Teams;

        private MatchType matchType;

        public MatchType Type {
            get 
            {
                return this.matchType;
            }
        }

        public Match(int numberTeams = 2, MatchType matchType = MatchType.Death)
        {
            this.Teams = new List<List<Player>>();
            this.matchType = matchType;
            for (int i=0; i<numberTeams; i++)
            {
                this.Teams.Add(new List<Player>());
            }
        }

        public int NumberTeams {
            get
            {
                return this.Teams.Count;
            }
        }

        public void AddPlayerToTeam(Player player, int team)
        {
            this.Teams[team].Add(player);
        }

        public List<Player> GetTeam(int team)
        {
            return this.Teams[team];
        }
    }
}
