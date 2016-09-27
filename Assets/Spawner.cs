using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

public class Spawner : NetworkedMonoBehavior {
    public GameObject player;

	// Use this for initialization
	protected override void NetworkStart () {
        Networking.Instantiate(player, NetworkReceivers.AllBuffered);
	}
}
