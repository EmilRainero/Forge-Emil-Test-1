using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using ScalableServer;
using System;

namespace UnityTest
{
    [TestFixture]
    internal class MatchTests
    {
        [Test]
        public void CreateTest()
        {
            Match match = new Match();
            Assert.NotNull(match);
            Assert.AreEqual(2, match.NumberTeams);
        }

        [Test]
        public void AddPlayerToTeamTest()
        {
            Match match = new Match();
            Player player = new Player();

            match.AddPlayerToTeam(player, 0);
            Assert.AreEqual(1, match.GetTeam(0).Count);
            Assert.AreEqual(0, match.GetTeam(1).Count);
        }


    }
}
