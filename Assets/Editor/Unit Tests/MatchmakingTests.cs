using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ScalableServer;
using System;

namespace UnityTest
{
    [TestFixture]
    internal class MatchmakingTests
    {
        [Test]
        public void CreateTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            Assert.NotNull(matchmaking);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MakeAMatchNotEnoughPlayersTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();

            matchmaking.MakeAMatch(players);
        }

        [Test]
        public void MakeAMatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            Player player1 = new Player();
            Player player2 = new Player();
            players.Add(player1);
            players.Add(player2);
            Match match = matchmaking.MakeAMatch(players);
            Assert.NotNull(match);
        }


        [Test]
        public void MakeAOneOnOneMatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            Player player1 = new Player();
            Player player2 = new Player();
            players.Add(player1);
            players.Add(player2);
            Match match = matchmaking.MakeAMatch(players);
            Assert.NotNull(match);
            List<Player> team0 = match.GetTeam(0);
            List<Player> team1 = match.GetTeam(1);
            Assert.AreEqual(1, team0.Count);
            Assert.AreEqual(1, team1.Count);
            Assert.AreEqual(player1, team0[0]);
            Assert.AreEqual(player2, team1[0]);
        }


        [Test]
        public void MakeA6on6MatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            for (int i=0; i<12; i++)
            {
                players.Add(new Player());
            }
            Match match = matchmaking.MakeAMatch(players, 6);
            Assert.NotNull(match);
            List<Player> team0 = match.GetTeam(0);
            List<Player> team1 = match.GetTeam(1);
            Assert.AreEqual(6, team0.Count);
            Assert.AreEqual(6, team1.Count);
        }


        [Test]
        public void MakeNTeamsMatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 20; i++)
            {
                players.Add(new Player(i.ToString()));
            }
            Match match = matchmaking.MakeAMatch(players, 5, 4);
            Assert.NotNull(match);
            Assert.AreEqual(4, match.NumberTeams);
            Assert.AreEqual(5, match.GetTeam(0).Count);
            Assert.AreEqual(5, match.GetTeam(1).Count);
            Assert.AreEqual(5, match.GetTeam(2).Count);
            Assert.AreEqual(5, match.GetTeam(3).Count);
        }

        private bool IncludeMethod_Even(Player player)
        {
            return int.Parse(player.Name) % 2 == 0;
        }

        [Test]
        public void MakeEvenOnlyPlayersMatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 20; i++)
            {
                players.Add(new Player(i.ToString()));
            }
            //Match match = matchmaking.MakeAMatch(players, 2, 2, IncludeMethod_Even);
            Match match = matchmaking.MakeAMatch(players, 2, 2, x => int.Parse(x.Name) % 2 == 0); // filter players - example uses even only
            Assert.NotNull(match);
            List<Player> team0 = match.GetTeam(0);
            List<Player> team1 = match.GetTeam(1);
            Assert.AreEqual(2, team0.Count);
            Assert.AreEqual(2, team1.Count);
            Assert.AreEqual("0", team0[0].Name);
            Assert.AreEqual("4", team0[1].Name);
            Assert.AreEqual("2", team1[0].Name);
            Assert.AreEqual("6", team1[1].Name);

        }

        private bool IncludeMethod_Prime(Player player)
        {
            return IsPrime(int.Parse(player.Name));
        }

        private bool IsPrime(int candidate)
        {
            // Test whether the parameter is a prime number.
            if ((candidate & 1) == 0)
            {
                if (candidate == 2)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            // Note:
            // ... This version was changed to test the square.
            // ... Original version tested against the square root.
            // ... Also we exclude 1 at the end.
            for (int i = 3; (i * i) <= candidate; i += 2)
            {
                if ((candidate % i) == 0)
                {
                    return false;
                }
            }
            return candidate != 1;
        }

        [Test]
        public void MakePrimeOnlyPlayersMatchTest()
        {
            Matchmaking matchmaking = new Matchmaking();
            List<Player> players = new List<Player>();
            for (int i = 0; i < 20; i++)
            {
                players.Add(new Player(i.ToString()));
            }
            Match match = matchmaking.MakeAMatch(players, 3, 2, IncludeMethod_Prime);
            Assert.NotNull(match);
            List<Player> team0 = match.GetTeam(0);
            List<Player> team1 = match.GetTeam(1);
            Assert.AreEqual(3, team0.Count);
            Assert.AreEqual(3, team1.Count);
            Assert.AreEqual("2", team0[0].Name);
            Assert.AreEqual("5", team0[1].Name);
            Assert.AreEqual("11", team0[2].Name);
            Assert.AreEqual("3", team1[0].Name);
            Assert.AreEqual("7", team1[1].Name);
            Assert.AreEqual("13", team1[2].Name);

        }

    }
}
