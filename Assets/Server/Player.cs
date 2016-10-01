using UnityEngine;
using System.Collections;

namespace ScalableServer
{

    public class Player
    {
        public string Name { get; set; }
        public object Data { get; set; }

        public Player(string name = "")
        {
            this.Name = name;
            this.Data = null;
        }

        public Player(object data)
        {
            this.Name = string.Empty;
            this.Data = data;
        }

    }
}
