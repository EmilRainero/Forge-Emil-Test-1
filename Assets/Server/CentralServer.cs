using UnityEngine;
using System.Collections;

namespace ScalableServer
{

    public class CentralServer
    {
        public Lobby Lobby { get; set; }

        public GameInstanceManager GameInstanceManager { get; set; }

        public CentralServer()
        {
        }


    }
}
