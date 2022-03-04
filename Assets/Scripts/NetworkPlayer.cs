using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkPlayer : MonoBehaviour
{
    // Boolean for whether client owns this player
    public bool isLocal = false;

    public void FixedUpdate()
    {
        // Send position to server if local object
        if (isLocal)
        {
            ObjectManager.Singleton.SendPosition(transform);
        }
    }
}
