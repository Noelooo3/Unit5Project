using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance => _instance;
    private static GameManager _instance;
    
    private Dictionary<ulong, string> _currentPlayers = new Dictionary<ulong, string>();

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this);
            return;
        }
        _instance = this;
    }

    public override void OnNetworkSpawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
    }

    private void OnPlayerConnected(ulong clientId)
    {
        if (!IsServer)
            return;
        _currentPlayers.TryAdd(clientId, "");
        Debug.Log($"Player: {clientId} joined the game");
    }

    public void OnNameUpdated(ulong clientId, string newName)
    {
        if (!IsServer)
            return;
        _currentPlayers[clientId] = newName;
        Debug.Log($"Player: {clientId} updated the name to: {newName}");
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
    }
}
