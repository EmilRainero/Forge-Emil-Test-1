using UnityEngine;
using System.Collections;
using NUnit.Framework;
using ScalableServer;
using System;

namespace UnityTest
{
    [TestFixture]
    internal class GameInstanceManagerTests
    {
        [Test]
        public void CreateTest()
        {
            GameInstanceManager gameInstanceManager = new GameInstanceManager();
            Assert.NotNull(gameInstanceManager);
            Assert.AreEqual(0, gameInstanceManager.NumberGameInstances);
        }

        [Test]
        public void AddGameInstanceTest()
        {
            GameInstanceManager gameInstanceManager = new GameInstanceManager();
            GameInstance gameInstance = new GameInstance();
            gameInstanceManager.AddGameInstance(gameInstance);
            Assert.AreEqual(1, gameInstanceManager.NumberGameInstances);
        }


        [Test]
        public void RemoveGameInstanceTest()
        {
            GameInstanceManager gameInstanceManager = new GameInstanceManager();
            GameInstance gameInstance = new GameInstance();
            gameInstanceManager.AddGameInstance(gameInstance);
            Assert.AreEqual(1, gameInstanceManager.NumberGameInstances);
            gameInstanceManager.RemoveGameInstance(gameInstance);
            Assert.AreEqual(0, gameInstanceManager.NumberGameInstances);
        }

        [Test]
        [ExpectedException(typeof(ArgumentException))]
        public void BadRemoveGameInstanceTest()
        {
            GameInstanceManager gameInstanceManager = new GameInstanceManager();
            GameInstance gameInstance = new GameInstance();
            gameInstanceManager.RemoveGameInstance(gameInstance);
        }


        [Test]
        public void AvailableGameInstancesTest()
        {
            GameInstanceManager gameInstanceManager = new GameInstanceManager();
            int numberInstances = 10;

            for (int i = 0; i < numberInstances; i++)
            {
                gameInstanceManager.AddGameInstance(new GameInstance());
            }
            Assert.AreEqual(numberInstances, gameInstanceManager.NumberGameInstances);

            GameInstance gameInstance = gameInstanceManager.GetGameInstance(0);
            GameInstance gameInstance2 = gameInstanceManager.GetGameInstance(1);

            gameInstance.StartMatch(new Match());
            gameInstance2.StartMatch(new Match());

            Assert.AreEqual(8, gameInstanceManager.AvailableGameInstances);
        }
    }
}