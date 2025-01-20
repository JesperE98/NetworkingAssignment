using System;
using TMPro;
using Unity.Netcode;
using UnityEngine;

public class Spawner : NetworkBehaviour
{
    private                  TMP_Text   text;
    [SerializeField] private GameObject[] prefabToSpawn;

    private GameObject    objectInstance;
    private NetworkObject networkObject;

    private void OnEnable() {
        
    }

    private void OnDisable() {
        
    }

    public override void OnNetworkSpawn() {
       
       SpawnNpcServerRpc();
        base.OnNetworkSpawn();
    }

    [ServerRpc(RequireOwnership = false)]
    void SpawnNpcServerRpc() {
       
       objectInstance = Instantiate(prefabToSpawn[0]);
       networkObject = objectInstance.GetComponent<NetworkObject>();
       networkObject.Spawn();
    }

    [ServerRpc(RequireOwnership = false)]
    public void PingServerRpc(Vector3 position) {
       objectInstance = Instantiate(prefabToSpawn[1]);
       networkObject  = objectInstance.GetComponent<NetworkObject>();
       networkObject.Spawn();
       networkObject.transform.position = position;
       
       Destroy(networkObject,2f);
    }
}
