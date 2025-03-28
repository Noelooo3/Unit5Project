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
        Vector3 lookPosition = Vector3.zero;
        if (Physics.Raycast(ray, out _hit, 1000f, _groundLayerMask))
        {
            lookPosition = _hit.point;
        }
        float vertical = Input.GetAxis("Vertical");
        MoveServerRpc(lookPosition, vertical * Time.deltaTime);
    }

    
    [ServerRpc] 
    private void MoveServerRpc(Vector3 lookPosition, float vertical)
    {
        this.gameObject.transform.LookAt(lookPosition);
        this.gameObject.transform.Translate(Vector3.forward * vertical * Speed);
    }
}
