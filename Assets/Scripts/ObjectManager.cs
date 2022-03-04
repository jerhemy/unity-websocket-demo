using System;
using System.Collections;
using System.Collections.Generic;
using Netcode;
using UnityEngine;

// Script for linking network messages with events in the game world
public class ObjectManager : MonoBehaviour
{
    public static ObjectManager Singleton;
    // Player to be spawned
    public GameObject playerPrefab;
    // Starting position for players 
    public Transform spawnPosition;

    // HashMap of player GameObjects keyed by their WebSocket id
    private Dictionary<String, GameObject> players = new Dictionary<string, GameObject>();

    public void Awake()
    {
        Singleton = this;
    }
    
    // Instantiates player based on given info
    public void SpawnPlayer(PositionInfo positionInfo)
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
        players.Add(positionInfo.owner, player);
        
        // Disable player controller script if not local player
        if (positionInfo.owner != ServerCommunication.Singleton.ClientID)
        {
            player.GetComponent<PlayerController>().enabled = false;
            // player.GetComponent<SphereCollider>().enabled = false;
        }
        else
        {
            player.GetComponent<NetworkPlayer>().isLocal = true;
        }
        
        Debug.Log("Player " + positionInfo.owner + " has been spawned!");
    }

    public void ReceivePosition(PositionInfo positionInfo)
    {
        string owner = positionInfo.owner;
        GameObject player;
        if (players.TryGetValue(owner, out player) && owner != ServerCommunication.Singleton.ClientID)
        {
            player.transform.position = positionInfo.position;
            player.transform.eulerAngles = positionInfo.rotation;
        }
    }

    public void SendPosition(Transform playerTransform)
    {
        Message message = new Message();
        message.title = "position";

        PositionInfo posInfo = new PositionInfo();
        posInfo.owner = ServerCommunication.Singleton.ClientID;
        posInfo.position = playerTransform.position;
        posInfo.rotation = playerTransform.eulerAngles;

        message.content = posInfo;

        string jsonMsg = JsonUtility.ToJson(message);
        
        Debug.Log(playerTransform.position);
        ServerCommunication.Singleton.SendRequest(jsonMsg);
    }
}
