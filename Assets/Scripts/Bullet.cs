using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Bullet : NetworkBehaviour
{
    [SerializeField] private NetworkObject networkObject;
    [SerializeField] private Rigidbody rigidbody;

    public void Spawn()
    {
        networkObject.Spawn();
    }
    
    public void Shoot(float force)
    {
        rigidbody.AddForce(transform.forward * force);
    }

    private void OnCollisionEnter(Collision other)
    {
        networkObject.Despawn();
    }
}
