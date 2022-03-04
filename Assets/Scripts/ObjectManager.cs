using System;
using System.Collections;
using System.Collections.Generic;
using Netcode;
using UnityEngine;

// Script for linking network messages with events in the game world
public class ObjectManager : MonoBehaviour
{
    // Player to be spawned
    public GameObject playerPrefab;
    // Starting position for players 
    public Transform spawnPosition;

    // HashMap of player GameObjects keyed by their WebSocket id
    private Dictionary<String, GameObject> players = new Dictionary<string, GameObject>();
    
    // Instantiates player based on given info
    public void SpawnPlayer(PositionInfo positionInfo)
    {
        GameObject player = GameObject.Instantiate(playerPrefab);
        players.Add(positionInfo.owner, player);
        
        // Disable player controller script if not local player
        if (positionInfo.owner != ServerCommunication.Singleton.ClientID)
        {
            player.GetComponent<PlayerController>().enabled = false;
        }
        
        Debug.Log("Player " + positionInfo.owner + " has been spawned!");
    }
}
