using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerShooting : NetworkBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootingForce = 3000f;

    private void Update()
    {
        if (!IsOwner)
            return;
        
        if (Input.GetMouseButtonDown(0))
        {
            // We should do an object pooling for the bullet
            Bullet bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            bullet.Spawn();
            bullet.Shoot(shootingForce);
        }
    }
}
