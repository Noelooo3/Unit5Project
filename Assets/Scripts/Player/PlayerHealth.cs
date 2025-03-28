using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public void TakeDamage(ulong shooterId)
    {
        if (!IsServer)
            return;
        if (shooterId == OwnerClientId)
            return;
        TakeDamageClientRpc();
    }

    [ClientRpc]
    private void TakeDamageClientRpc()
    {
        Debug.Log(OwnerClientId + " is taking damage");
    }
}
