using UnityEngine;
using System.Collections;
using System;

namespace ScalableServer
{

    public class GameInstance
    {
        private bool matchRunning = false;
        public bool isMatchRunning { get
            {
                return matchRunning;
            }
        }

        public Match Match { get; set; }

        public void StartMatch(Match match)
        {
            this.Match = match;
            matchRunning = true;
        }

        public void EndMatch()
        {
            matchRunning = false;
        }
    }
}
