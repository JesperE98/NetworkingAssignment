using Unity.Netcode;
using UnityEngine;
using Assets.MyFolder.Scripts.PlayerScripts;

public class ClientPlayerMove : NetworkBehaviour {

   [SerializeField] private Player _player;
   private void Awake() {
      _player.enabled = false;
   }

   public override void OnNetworkSpawn() {
      base.OnNetworkSpawn();
      
      enabled = IsClient; // Enable if this is a client.
      if (!IsOwner) {
         // Disable if this is not the owner
         enabled         = false;
         _player.enabled = false;
         return;
      }
      
      _player.enabled = true;
   }
}
