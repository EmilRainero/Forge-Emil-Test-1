using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;

public class TestNetworking : MonoBehaviour
{
    public Text Text;

    private string lobbyIpAddress = "127.0.0.1";
    private string gimIpAddress = "127.0.0.1";
    private string lobbyPort = "15937";
    private string gicPort = "15937";
    private string gimPort = "16100";
    private string giPort = "16200";
    private string clientPort = "16300";

    private string started = null;
    private GUIStyle guiStyle = new GUIStyle();

    void Start()
    {
        DebugLog.SetText(Text);
        guiStyle.fontSize = 16;
    }

    void OnGUI()
    {
        if (started == null)
        {
            //clientPort = GUI.TextField(new Rect(Screen.width - 300, 130, 100, 30), clientPort);

            if (GUI.Button(new Rect(10, 10, 120, 30), "Start Server"))
            {
                Debug.Log("Start Lobby  port: " + lobbyPort);
                started = "Lobby";
                StartLobby(lobbyIpAddress, ushort.Parse(lobbyPort));
            }
            GUI.Label(new Rect(110, 50, 100, 30), "IP Address", guiStyle);
            lobbyIpAddress = GUI.TextField(new Rect(10, 50, 90, 20), lobbyIpAddress);
            GUI.Label(new Rect(110, 80, 100, 20), "Lobby Port", guiStyle);
            lobbyPort = GUI.TextField(new Rect(10, 80, 90, 20), lobbyPort);
            GUI.Label(new Rect(110, 110, 100, 20), "GICluster Port", guiStyle);
            gicPort = GUI.TextField(new Rect(10, 110, 90, 20), gicPort);

            if (GUI.Button(new Rect(Screen.width - 210, 10, 190, 30), "Start GI Manager"))
            {
                Debug.Log("Start GI Manager");
                started = "GI Manager";
                StartGameInstanceManager(lobbyIpAddress, ushort.Parse(gimPort));
            }
            gimIpAddress = GUI.TextField(new Rect(Screen.width - 210, 50, 90, 20), gimIpAddress);
            gimPort = GUI.TextField(new Rect(Screen.width - 210, 80, 90, 20), gimPort);
            GUI.Label(new Rect(Screen.width - 110, 50, 100, 20), "IP Address", guiStyle);
            GUI.Label(new Rect(Screen.width - 110, 80, 100, 20), "Port", guiStyle);

            if (GUI.Button(new Rect(Screen.width - 210, 150, 190, 30), "Start GI"))
            {
                Debug.Log("Start GI");
                started = "GI";
                StartGameInstance(lobbyIpAddress, ushort.Parse(giPort));
            }
            giPort = GUI.TextField(new Rect(Screen.width - 210, 190, 90, 20), giPort);
            GUI.Label(new Rect(Screen.width - 110, 190, 100, 20), "Port", guiStyle);

            if (GUI.Button(new Rect(10, 150, 190, 30), "Start Client"))
            {
                DebugLog.Log("Start Client");
                started = "Client";
                StartClient(lobbyIpAddress, ushort.Parse(lobbyPort));
            }
        }
        else
        {
            if (started == "Client" && GUI.Button(new Rect(Screen.width - 200, 50, 190, 30), "Disconnect"))
            {
                clientNetworked.Disconnect();
                started = null;
            }
        }
    }

    private ClientNetworked clientNetworked;

    private void StartLobby(string lobbyIpAddress, ushort port)
    {
        LobbyNetworked lobbyNetworked = new LobbyNetworked();
        lobbyNetworked.Connect(lobbyIpAddress, port, Networking.TransportationProtocolType.TCP);
    }

    private void StartGameInstanceManager(string lobbyIpAddress, ushort port)
    {
        GameInstanceManagerNetworked gameInstanceManagerNetworked = new GameInstanceManagerNetworked();
        gameInstanceManagerNetworked.Connect(port, Networking.TransportationProtocolType.TCP);
    }

    private void StartGameInstance(string lobbyIpAddress, ushort port)
    {
        GameInstanceNetworked gameInstanceNetworked = new GameInstanceNetworked();
        gameInstanceNetworked.Connect(port, Networking.TransportationProtocolType.TCP);
    }

    private void StartClient(string lobbyIpAddress, ushort lobbyPort)
    {
        if (clientNetworked == null)
        {
            clientNetworked = new ClientNetworked();
        }
        clientNetworked.Connect(lobbyIpAddress, lobbyPort, Networking.TransportationProtocolType.TCP);
    }
}
