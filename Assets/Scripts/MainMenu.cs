using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject panel;
    [SerializeField] private ServerCommunication serverHandler;

    void Awake()
    {
        serverHandler.OnConnected.AddListener(HidePanel);
    }

    private void HidePanel()
    {
        panel.SetActive(false);
    }
}
