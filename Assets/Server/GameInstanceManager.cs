using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace ScalableServer
{

    public class GameInstanceManager
    {
        public int NumberGameInstances {
            get
            {
                return this.gameInstances.Count;
            }
        }

        public int AvailableGameInstances {
            get
            {
                int count = 0;

                foreach (GameInstance instance in this.gameInstances)
                {
                    if (!instance.isMatchRunning)
                    {
                        count++;
                    }
                }
                return count;
            }
        }

        private List<GameInstance> gameInstances;

        public GameInstanceManager()
        {
            gameInstances = new List<GameInstance>();
        }

        public void AddGameInstance(GameInstance gameInstance)
        {
            this.gameInstances.Add(gameInstance);
        }

        public void RemoveGameInstance(GameInstance gameInstance)
        {
            if (this.gameInstances.Contains(gameInstance))
            {
                this.gameInstances.Remove(gameInstance);
            }
            else
            {
                throw new ArgumentException("not found");
            }
        }

        public GameInstance GetGameInstance(int index)
        {
            return this.gameInstances[index];
        }
    }
}
