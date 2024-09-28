using Unity.Netcode;
using UnityEngine;

namespace _Scripts
{
    public class PlayerHand : NetworkBehaviour
    {
        private Color color;
        public bool anyObjectEquipped;
        
        private Interactable _equippedInteractable;

        public override void OnNetworkSpawn()
        {
            if (!IsOwner) return;
            color = new Color(Random.value, Random.value, Random.value);
        }
        public void Pickup(Interactable interactable)
        {
            TransformLocker transformLocker = interactable.GetComponent<TransformLocker>();
            transformLocker.Lock(transform);
            
            interactable.isEquipped.Value = true;
            interactable.color.Value = color;
            interactable.isColliderEnabled.Value = false;
            interactable.SetRigidbodyKinematic(true);
            anyObjectEquipped = true;
            _equippedInteractable = interactable;
            Debug.Log("Picked up object");
        }
        
        
        public void Drop()
        {
            TransformLocker transformLocker = _equippedInteractable.GetComponent<TransformLocker>();
            transformLocker.Unlock();
            
            _equippedInteractable.isEquipped.Value = false;
            _equippedInteractable.color.Value = Color.green;
            _equippedInteractable.isColliderEnabled.Value = true;
            _equippedInteractable.SetRigidbodyKinematic(false);
            anyObjectEquipped = false;
            ChangeOwnershipServerRpc();
            _equippedInteractable = null;
            Debug.Log("Dropped object");
        }
        
        [ServerRpc]
        private void ChangeOwnershipServerRpc()
        {
            if(!IsOwner || IsServer) return; 
            _equippedInteractable.NetworkObject.ChangeOwnership(MultiplayerGameManager.Instance.GetServerClientId());
        }
        
        
    }
}
