using Unity.Netcode;
using UnityEngine;

public class DisableObjectOnStart : NetworkBehaviour
{

   void Update() {
      enabled = IsServer;
        
      if(!enabled)
         return;
        
      this.enabled = false;
   }

    
}
