using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private float shootingForce = 3000f;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // We should do an object pooling for the bullet
            Bullet bullet = Instantiate(bulletPrefab, shootingPoint.position, shootingPoint.rotation);
            bullet.Shoot(shootingForce);
        }
    }
}
