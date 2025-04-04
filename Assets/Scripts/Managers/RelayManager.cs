using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class RelayManager : MonoBehaviour
{
    public static RelayManager Instance => _instance;
    private static RelayManager _instance;

    private bool _isSignedIn = false;
    
    [SerializeField] private UnityTransport _unityTransport;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _instance = this;
    }

    private async void Start()
    {
        await UnityServices.InitializeAsync();
        
        AuthenticationService.Instance.SignedIn += OnSignedIn;
        await AuthenticationService.Instance.SignInAnonymouslyAsync();
    }

    private void OnSignedIn()
    {
        Debug.Log("Signed in completed.");
        _isSignedIn = true;
        AuthenticationService.Instance.SignedIn -= OnSignedIn;
    }

    public async void CreateRelay()
    {
        try
        {
            Allocation allocation = await RelayService.Instance.CreateAllocationAsync(9);
            string joinCode = await RelayService.Instance.GetJoinCodeAsync(allocation.AllocationId);
            UIController.Instance.SetCode(joinCode);
            _unityTransport.SetHostRelayData(
                allocation.RelayServer.IpV4,
                (ushort) allocation.RelayServer.Port,
                allocation.AllocationIdBytes,
                allocation.Key,
                allocation.ConnectionData);
            NetworkManager.Singleton.StartHost();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            UIController.Instance.OnGameOver();
        }
    }

    public async void JoinRelay(string joinCode)
    {
        try
        {
            JoinAllocation joinAllocation = await RelayService.Instance.JoinAllocationAsync(joinCode);
            _unityTransport.SetClientRelayData(
                joinAllocation.RelayServer.IpV4,
                (ushort) joinAllocation.RelayServer.Port,
                joinAllocation.AllocationIdBytes,
                joinAllocation.Key,
                joinAllocation.ConnectionData,
                joinAllocation.HostConnectionData);
            NetworkManager.Singleton.StartClient();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
            UIController.Instance.OnGameOver();
        }
    }
}
