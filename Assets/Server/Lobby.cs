using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ScalableServer
{

    public class Lobby
    {
        public int NumberPlayers
        {
            get
            {
                return players.Count;
            }
        }

        private List<Player> players;
        private int maxPlayers;

        public Lobby(int maxPlayers = int.MaxValue)
        {
            this.maxPlayers = maxPlayers;
            this.players = new List<Player>();
        }

        public void AddPlayer(Player player)
        {
            if (this.NumberPlayers >= this.maxPlayers)
                throw new ArgumentException("max number of players");
            players.Add(player);
        }

        public void RemovePlayer(Player player)
        {
            if (players.Contains(player))
            {
                players.Remove(player);
            }
            else
            {
                throw new ArgumentException("not found");
            }
        }

        public void RemoveAllPlayers()
        {
            players.Clear();
        }
    }
}
