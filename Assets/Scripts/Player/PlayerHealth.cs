using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    [SerializeField] private Transform healthBarAnchorPoint;
    private NetworkVariable<int> _health = new NetworkVariable<int>(100);

    public override void OnNetworkSpawn()
    {
        _health.OnValueChanged += OnHealthValueChanged;
        healthBarAnchorPoint.localScale = new Vector3(_health.Value / 100f, 1f, 1f);
    }

    public void TakeDamage(ulong shooterId)
    {
        if (!IsServer)
            return;
        if (shooterId == OwnerClientId)
            return;
        _health.Value -= 20;
        if (_health.Value <= 0)
        {
            this.NetworkObject.Despawn();
            GameManager.Instance.OnPlayerDespawned(OwnerClientId);
        }
    }

    private void OnHealthValueChanged(int oldValue, int newValue)
    {
        healthBarAnchorPoint.localScale = new Vector3(_health.Value / 100f, 1f, 1f);
    }
    
    public override void OnNetworkDespawn()
    {
        _health.OnValueChanged -= OnHealthValueChanged;
    }
}
