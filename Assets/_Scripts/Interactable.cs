using Unity.Netcode;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public sealed class Interactable : NetworkBehaviour
    {
        private MeshRenderer _meshRenderer;
        private Collider[] _colliders;
        private Rigidbody _rigidbody;
        
        public NetworkVariable<bool> isRigidbodyKinematic = new(false,readPerm: NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isColliderEnabled = new(true,readPerm: NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<bool> isEquipped = new(false,readPerm: NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner);
        public NetworkVariable<Color> color = new(Color.red,readPerm: NetworkVariableReadPermission.Everyone, NetworkVariableWritePermission.Owner); 
        
        
        public override void OnNetworkSpawn()
        {
            _meshRenderer = GetComponentInChildren<MeshRenderer>();
            _colliders = GetComponentsInChildren<Collider>();
            _rigidbody = GetComponent<Rigidbody>();
            
            isRigidbodyKinematic.OnValueChanged += UpdateInteractableRigidbodyData;
            isColliderEnabled.OnValueChanged += UpdateInteractableColliderData;
            color.OnValueChanged += UpdateInteractableColorData;
            
            SyncObject();
        }


        private void UpdateInteractableColorData(Color previousValue, Color newValue)
        {
            _meshRenderer.material.color = newValue;
        }
        private void UpdateInteractableRigidbodyData(bool previousValue, bool newValue)
        {
            _rigidbody.isKinematic = newValue;
        }
        private void UpdateInteractableColliderData(bool previousValue, bool newValue)
        {
            foreach (var col in _colliders)
            {
                col.enabled = newValue;
            }
        }
        
        private void SyncObject()
        {
            foreach (var col in _colliders)
            {
                col.enabled = isColliderEnabled.Value;
            }
            _rigidbody.isKinematic = isRigidbodyKinematic.Value;
            _meshRenderer.material.color = color.Value;
        }

        public void Interact()
        {
            RequestInteractServerRpc();
        }
        
        
        public void SetRigidbodyKinematic(bool value)
        {
            _rigidbody.isKinematic = value;
            UpdateInteractableRigidbodyData(isRigidbodyKinematic.Value,value);
        }
        
        
        [ServerRpc(RequireOwnership = false)]
        private void RequestInteractServerRpc(ServerRpcParams rpcParams = default)
        {
            ulong clientId = rpcParams.Receive.SenderClientId;
            if(NetworkObject.OwnerClientId != clientId) NetworkObject.ChangeOwnership(clientId);
            Debug.Log("Interacted with " + gameObject.name);
            InteractClientRpc(clientId);
        }

        [Rpc(SendTo.Owner)]
        private void InteractClientRpc(ulong clientId)
        {//
            PlayerNetwork playerNetwork = MultiplayerGameManager.Instance.GetPlayerNetworkById(clientId);
            PlayerHand playerHand = playerNetwork.playerHand;
            playerHand.Pickup(this);
        }


    }
}
