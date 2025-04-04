using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        _currentPlayers[clientId] = newName;
        Debug.Log($"Player: {clientId} updated the name to: {newName}");
    }

    public void OnPlayerDespawned(ulong clientId)
    {
        if (!IsServer)
            return;
        _currentPlayers.Remove(clientId);

        if (_currentPlayers.Count > 1)
            return;

        // string remainingPlayerName = _currentPlayers.First().Value;
        // This is the same as the following
        foreach (var player in _currentPlayers)
        {
            GameOverClientRpc(player.Value);
            break;
        }
        _currentPlayers.Clear();
    }

    [ClientRpc]
    private void GameOverClientRpc(string playerName)
    {
        Debug.Log($"Game over, the winner is {playerName}!");
        UIController.Instance.OnGameOver();
        NetworkManager.Singleton.Shutdown();
    }

    public override void OnNetworkDespawn()
    {
        NetworkManager.Singleton.OnClientConnectedCallback -= OnPlayerConnected;
    }
}
