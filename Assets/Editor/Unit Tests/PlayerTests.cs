using UnityEngine;
using System.Collections;
using NUnit.Framework;
using ScalableServer;

namespace UnityTest
{
    [TestFixture]
    internal class PlayerTests
    {
        [Test]
        public void CreateTest()
        {
            string name = "Emil";
            Player player = new Player(name);
            Assert.NotNull(player);
            Assert.AreEqual(name, player.Name);
        }

        [Test]
        public void NameTest()
        {
            string name = "Emil";
            string newName = "Rainero";

            Player player = new Player(name);
            Assert.NotNull(player);
            Assert.AreEqual(name, player.Name);

            player.Name = newName;
            Assert.AreEqual(newName, player.Name);
        }
    }
}