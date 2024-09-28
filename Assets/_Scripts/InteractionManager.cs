using TMPro;
using Unity.Netcode;
using UnityEngine;

namespace _Scripts
{
    public class InteractionManager : NetworkBehaviour
    {
        private Camera _camera;
        private TextMeshProUGUI _interactionText;
        private float _rayDistance = 10F;
        [SerializeField] LayerMask _layerMask;

        public override void OnNetworkSpawn()
        {
            if(!IsOwner) return;
            _camera = GetComponent<Camera>();
            _interactionText = GameObject.FindWithTag("InteractText").GetComponent<TextMeshProUGUI>();
            Debug.Log("InteractionManager spawned");
        }

        void Update()
        {
            if(!IsOwner)
            {
                return;
            }
            
            if(Input.GetKeyDown(KeyCode.X))
            {
                PlayerHand playerHand = MultiplayerGameManager.Instance.GetLocalPlayerNetwork().playerHand;
                if(playerHand.anyObjectEquipped)
                {
                    playerHand.Drop();
                }
            }
        
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
            Debug.DrawRay(ray.origin, ray.direction * _rayDistance, Color.red);

            if (Physics.Raycast(ray, out RaycastHit hit, _rayDistance,layerMask: _layerMask))
            {
                Interactable interactable = hit.collider.GetComponent<Interactable>();
                if (interactable != null)
                {
                    if (interactable.isEquipped.Value)
                    {
                        _interactionText.enabled = false;
                        return;
                    }
                    
                    if(MultiplayerGameManager.Instance.GetLocalPlayerNetwork().playerHand.anyObjectEquipped)
                    {
                        _interactionText.enabled = false;
                        return;
                    }
                    
                    _interactionText.enabled = true;
                    
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        interactable.Interact();
                    }
                }
                else
                {
                    _interactionText.enabled = false;
                }
            }
            else
            {
                _interactionText.enabled = false;
            }

        }
    }
}
