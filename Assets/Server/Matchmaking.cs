using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

namespace ScalableServer
{
    public class Matchmaking
    {
        public Matchmaking()
        {

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
