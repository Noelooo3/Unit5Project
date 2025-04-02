using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections;
using Unity.Netcode;
using UnityEngine;

public class PlayerInfo : NetworkBehaviour
{
    [SerializeField] private TMP_Text playerNameDisplay;
    private NetworkVariable<FixedString64Bytes> playerName 
        = new NetworkVariable<FixedString64Bytes>("default", NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);

    public override void OnNetworkSpawn()
    {
        // To make sure we get notified when the name is changed.
        playerName.OnValueChanged += OnNameValueChanged;
        playerNameDisplay.text = playerName.Value.ToString();
        
        if (!IsOwner)
            return;
        playerName.Value = new FixedString64Bytes(UIController.Instance.GetName());
    }

    private void OnNameValueChanged(FixedString64Bytes previousValue, FixedString64Bytes newValue)
    {
        if (previousValue == newValue)
            return;
        playerNameDisplay.text = playerName.Value.ToString();
        if (!IsServer)
            return;
        GameManager.Instance.OnNameUpdated(OwnerClientId, playerName.Value.ToString());
    }

    public override void OnNetworkDespawn()
    {
        playerName.OnValueChanged -= OnNameValueChanged;
    }
}
