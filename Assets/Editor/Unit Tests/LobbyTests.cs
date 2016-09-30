using System;
using System.Collections.Generic;
using System.Threading;
using NUnit.Framework;
using UnityEngine;
using ScalableServer;

namespace UnityTest
{
    [TestFixture]
    internal class LobbyTests
    {
        [Test]
        public void CreateTest()
        {
            Assert.NotNull(new Lobby());
        }

        [Test]
        public void InitiallyNoPlayersTest()
        {
            Lobby lobby = new Lobby();
            Assert.AreEqual(0, lobby.NumberPlayers);
        }

        [Test]
        public void AddFirstPlayerTest()
        {
            Lobby lobby = new Lobby();
            Player player = new Player();
            lobby.AddPlayer(player);
            Assert.AreEqual(1, lobby.NumberPlayers);
        }

        [Test]
        public void RemoveFirstPlayerTest()
        {
            Lobby lobby = new Lobby();
            Player player = new Player();
            lobby.AddPlayer(player);
            Assert.AreEqual(1, lobby.NumberPlayers);
            lobby.RemovePlayer(player);
            Assert.AreEqual(0, lobby.NumberPlayers);
        }


        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void RemoveMissingPlayerTest()
        {
            Lobby lobby = new Lobby();
            Player player1 = new Player();
            Player player2 = new Player();
            lobby.AddPlayer(player1);
            lobby.RemovePlayer(player2);
        }


        [Test]
        public void RemoveAllPlayersTest()
        {
            Lobby lobby = new Lobby();
            Player player1 = new Player();
            Player player2 = new Player();
            Player player3 = new Player();
            lobby.AddPlayer(player1);
            lobby.AddPlayer(player2);
            lobby.AddPlayer(player3);
            lobby.RemoveAllPlayers();
            Assert.AreEqual(0, lobby.NumberPlayers);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void MaxPlayerTest()
        {
            Lobby lobby = new Lobby(1);
            Player player1 = new Player();
            Player player2 = new Player();
            lobby.AddPlayer(player1);
            lobby.AddPlayer(player2);
        }
    }
}