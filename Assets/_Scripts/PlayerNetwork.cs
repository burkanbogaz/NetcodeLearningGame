using Unity.Netcode;
using UnityEngine;

namespace _Scripts
{
    public class PlayerNetwork : NetworkBehaviour
    {
        public PlayerHand playerHand;
        
        public override void OnNetworkSpawn()
        {
            playerHand = GetComponentInChildren<PlayerHand>();
            
            if(!IsOwner)
            {
                PlayerMovement playerMovement = GetComponent<PlayerMovement>();
                Camera playerCamera = GetComponentInChildren<Camera>();
                AudioListener audioListener = GetComponentInChildren<AudioListener>();
                Destroy(playerMovement);
                playerCamera.enabled = false;
                Destroy(audioListener);
            }
            
        }
        
        
    }
}
