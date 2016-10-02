using UnityEngine;
using System.Collections;
using NUnit.Framework;
using ScalableServer;

namespace UnityTest
{
    [TestFixture]
    internal class GameInstanceTests
    {
        [Test]
        public void CreateTest()
        {
            GameInstanceNetworked gameInstance = new GameInstanceNetworked();
            Assert.NotNull(gameInstance);
        }

        [Test]
        public void StartMatchTest()
        {
            GameInstanceNetworked gameInstance = new GameInstanceNetworked();
            Match match = new Match();
            match.AddPlayerToTeam(new Player(), 0);
            match.AddPlayerToTeam(new Player(), 1);
            gameInstance.StartMatch(match);
            Assert.AreEqual(match, gameInstance.Match);
        }


        [Test]
        public void EndMatchTest()
        {
            GameInstanceNetworked gameInstance = new GameInstanceNetworked();
            Match match = new Match();
            match.AddPlayerToTeam(new Player(), 0);
            match.AddPlayerToTeam(new Player(), 1);
            gameInstance.StartMatch(match);
            gameInstance.EndMatch();
            Assert.IsFalse(gameInstance.isMatchRunning);
        }

    }
}