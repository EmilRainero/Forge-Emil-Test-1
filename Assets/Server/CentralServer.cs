using UnityEngine;
using System.Collections;
using BeardedManStudios.Network;

namespace ScalableServer
{

    public class CentralServer
    {
        private LobbyListener lobbyListener;
        private Lobby lobby;

        private GameInstanceManager gameInstanceManager;

        public CentralServer()
        {
        }

        public void Setup()
        {
            this.lobby = new Lobby();
            this.lobbyListener = new LobbyListener(this.lobby);
            this.lobbyListener.Connect(17000, Networking.TransportationProtocolType.TCP);

            this.gameInstanceManager = new GameInstanceManager();
            GameInstance gameInstance = new GameInstance();
            this.gameInstanceManager.AddGameInstance(gameInstance);
        }
    }
}
