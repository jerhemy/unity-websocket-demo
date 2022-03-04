using System;
using Netcode;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;

public class ServerCommunication : MonoBehaviour
{
    // Server IP address
    [SerializeField]
    private string hostIP = "localhost";

    // Server port
    [SerializeField]
    private int port = 3000;

    // Flag to use localhost
    [SerializeField]
    private bool useLocalhost = false;

    // Reference to ObjectManager
    [SerializeField] private ObjectManager _objectManager;

    // Address used in code
    private string host => useLocalhost ? "localhost" : hostIP;
    
    // Final server address
    private string server;

    // WebSocket Client
    private WsClient client;

    [SerializeField] private TMP_InputField inputField;

    [HideInInspector]
    public string ClientID; // ClientID sent by server
    
    public static ServerCommunication Singleton; // Singleton for access from other scripts

    public UnityEvent OnConnected;
    
    /// <summary>
    /// Unity method called on initialization
    /// </summary>
    private void Awake()
    {
        client = new WsClient();
        Singleton = this;
    }

    /// <summary>
    /// Unity method called every frame
    /// </summary>
    private void Update()
    {
        // Check if server send new messages
        var cqueue = client.receiveQueue;
        string msg;
        while (cqueue.TryPeek(out msg))
        {
            // Parse newly received messages
            cqueue.TryDequeue(out msg);
            HandleMessage(msg);
        }
    }

    /// <summary>
    /// Method responsible for handling server messages
    /// </summary>
    /// <param name="msg">Message.</param>
    private void HandleMessage(string msg)
    {
        // Deserialize message from string
        Message message = JsonUtility.FromJson<Message>(msg);
        
        // Act depending on title of message
        switch (message.title)
        {
            case "client_id": // Receive ID from server
                ClientID = message.content.owner;
                break;
            case "position":
                _objectManager.ReceivePosition(message.content);
                break;
            case "spawn":
                _objectManager.SpawnPlayer(message.content);
                break;
            default:
                Debug.Log("Server: " + msg);
                break;
        }
    }

    public void SetIP(string hostIP)
    {
        this.hostIP = hostIP;
        Debug.Log(hostIP);
    }

    public void SetPort(string port)
    {
        this.port = int.Parse(port);
    }

    /// <summary>
    /// Call this method to connect to the server
    /// </summary>
    public async void ConnectToServer()
    {
        server = "ws://" + host + ":" + port;
        Debug.Log("Connecting to: " + server);
        await client.Connect(server, OnConnected.Invoke);
    }

    /// <summary>
    /// Method which sends data through websocket
    /// </summary>
    /// <param name="message">Message.</param>
    public void SendRequest(string message)
    {
        client.Send(message);
    }
    
    public void SendText()
    {
        client.Send(inputField.text);
    }
}