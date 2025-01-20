using System;
using System.Collections;
using Unity.Netcode;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class RespawnTrigger : NetworkBehaviour
{
    [SerializeField]
    private Transform respawnPoint;

    
    private BoxCollider _collider;

    public override void OnNetworkSpawn() {
        _collider = GetComponent<BoxCollider>();

        _collider.enabled   = true;
        _collider.isTrigger = true;
        _collider.size      = new Vector3(100, 5, 100);
        
        base.OnNetworkSpawn();
    }

    
    private void OnTriggerEnter(Collider other) {
        if (other.tag == "Player") {
            
            print("Respawning player");
            
            other.transform.position = respawnPoint.position;
        }
    }
}
