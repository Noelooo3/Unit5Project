using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerMovement : NetworkBehaviour
{
    [SerializeField] private float Speed = 15f;

    private LayerMask _groundLayerMask;
    private RaycastHit _hit;
    
    private void Start()
    
    {
        _groundLayerMask = LayerMask.GetMask("Ground");
    }

    void Update()
    {
        if (!IsOwner)
            return;
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit, 1000f, _groundLayerMask))
        {
            this.gameObject.transform.LookAt(_hit.point);
        }
        
        float vertical = Input.GetAxis("Vertical");
        this.gameObject.transform.Translate(Vector3.forward * vertical * Speed * Time.deltaTime);
    }
}
