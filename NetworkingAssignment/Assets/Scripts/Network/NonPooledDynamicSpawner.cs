using Unity.Netcode;
using UnityEngine;

public class NonPooledDynamicSpawner : NetworkBehaviour
{
    #region Variables

      public  GameObject PrefabToSpawn;
      public  bool       DestroyWithSpawner;
      private GameObject m_PrefabInstance;

    #endregion
       
   #region Network Variables
      
      private NetworkObject m_SpawnedNetworkObject;
      
      #endregion

   public override void OnNetworkSpawn() {
      enabled = IsServer;

      if (!enabled || PrefabToSpawn == null)
         return;
      
      m_PrefabInstance = Instantiate(PrefabToSpawn);
      
      m_SpawnedNetworkObject = m_PrefabInstance.GetComponent<NetworkObject>();
      m_SpawnedNetworkObject.Spawn();
   }

   public override void OnNetworkDespawn() {
      if (IsServer && DestroyWithSpawner 
                   && m_SpawnedNetworkObject != null 
                   && m_SpawnedNetworkObject.IsSpawned) {
         m_SpawnedNetworkObject.Despawn();
      }
      
      base.OnNetworkDespawn();
   }
}
