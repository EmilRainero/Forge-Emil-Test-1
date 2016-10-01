using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BeardedManStudios.Network;
using ScalableServer;

public class TestNetworking : MonoBehaviour
{
    public Text Text;

    private string ipAddress = "127.0.0.1";
    private string lobbyPort = "15937";
    private string gimPort = "16100";
    private string giPort = "16200";
    private string clientPort = "16300";

    private string started = null;

    void Start()
    {
        DebugLog.SetText(Text);
    }

    void OnGUI()
    {
        if (started == null)
        {
            ipAddress = GUI.TextField(new Rect(Screen.width - 300, 10, 100, 30), ipAddress);
            lobbyPort = GUI.TextField(new Rect(Screen.width - 300, 50, 100, 30), lobbyPort);
            gimPort = GUI.TextField(new Rect(Screen.width - 300, 90, 100, 30), gimPort);
            giPort = GUI.TextField(new Rect(Screen.width - 300, 130, 100, 30), giPort);
            clientPort = GUI.TextField(new Rect(Screen.width - 300, 170, 100, 30), clientPort);

            if (GUI.Button(new Rect(Screen.width - 200, 50, 190, 30), "Start Lobby"))
            {
                Debug.Log("Start Lobby  port: " + lobbyPort);
                started = "Lobby";
                StartLobby(ipAddress, ushort.Parse(lobbyPort));
            }
            if (GUI.Button(new Rect(Screen.width - 200, 90, 190, 30), "Start GI Manager"))
            {
                Debug.Log("Start GI Manager");
                started = "GI Manager";
            }
            if (GUI.Button(new Rect(Screen.width - 200, 130, 190, 30), "Start GI"))
            {
                Debug.Log("Start GI");
                started = "GI";
            }
            if (GUI.Button(new Rect(Screen.width - 200, 170, 190, 30), "Start Client"))
            {
                DebugLog.Log("Start Client");
                started = "Client";
                StartClient(ipAddress, ushort.Parse(lobbyPort));
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

    private void StartClient(string lobbyIpAddress, ushort lobbyPort)
    {
        if (clientNetworked == null)
        {
            clientNetworked = new ClientNetworked();
        }
        clientNetworked.Connect(lobbyIpAddress, lobbyPort, Networking.TransportationProtocolType.TCP);
    }
}
