using UnityEngine;
using System.Collections;

namespace ScalableServer
{

    public class Player
    {
        public string Name { get; set; }

        public Player(string name = "")
        {
            this.Name = name;
        }

    }
}
