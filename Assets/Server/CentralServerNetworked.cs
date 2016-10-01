using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;

public class CentralServerNetworked<P> : NetworkedMonoBehavior {

    //private CentralServer CentralServer { get; set; }
    //private LobbyNetworked LobbyNetworked { get; set; }
    //private Lobby<P> Lobby { get; set; }
    //private GameInstanceManager GameInstanceManager { get; set; }
    //private GameInstanceManagerNetworked GameInstanceManagerNetworked { get; set; }
    //private GameInstanceNetworked GameInstanceNetworked { get; set; }
    //public string HOST = "127.0.0.1";
    //public Networking.TransportationProtocolType PROTOCOL_TYPE = Networking.TransportationProtocolType.TCP;
    //public ushort LOBBY_PORT = 16000;
    //public ushort GAME_INSTANCE_MANAGER_PORT = 16100;
    //public ushort GAME_INSTANCE_PORT = 16200;

    //// Use this for initialization
    //void Start () {

    //    this.CentralServer = new CentralServer();
    //    this.Lobby = new Lobby<P>();
    //    GameInstance gameInstance = new GameInstance();
    //    this.GameInstanceManager = new GameInstanceManager();
    //    this.GameInstanceManager.AddGameInstance(gameInstance);


    //    this.LobbyNetworked = new LobbyNetworked(this.Lobby);
    //    this.GameInstanceManagerNetworked = new GameInstanceManagerNetworked(this.GameInstanceManager);
    //    this.GameInstanceNetworked = new GameInstanceNetworked(gameInstance);

    //    this.LobbyNetworked.Connect(HOST, LOBBY_PORT, PROTOCOL_TYPE);
    //    this.GameInstanceManagerNetworked.Connect(HOST, GAME_INSTANCE_MANAGER_PORT, PROTOCOL_TYPE);
    //    this.GameInstanceNetworked.Connect(HOST, GAME_INSTANCE_PORT, PROTOCOL_TYPE);


    //}

}
