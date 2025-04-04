using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Rigidbody rigidbody;

    private ulong _shooterId;

    public void Spawn(ulong shooterId, float force)
    {
        _shooterId = shooterId;
        networkObject.Spawn();
        rigidbody.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (!IsServer)
            return;
        networkObject.Despawn();
        if (!other.gameObject.CompareTag("Player"))
            return;
        PlayerHealth playerHealth = other.gameObject.GetComponent<PlayerHealth>();
        if (playerHealth == null)
            return;
        playerHealth.TakeDamage(_shooterId);
    }
}
