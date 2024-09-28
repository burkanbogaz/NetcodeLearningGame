using System;
using Unity.Netcode;

namespace _Scripts
{
    public class MultiplayerGameManager : NetworkBehaviour
    {
        public static MultiplayerGameManager Instance { get; private set; }
        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            
        }

        private void Start()
        {
            NetworkManager.Singleton.OnServerStarted += OnServerStarted;
        }

        private void OnServerStarted()
        {
            GameManager.Instance.SpawnObjects();
        }


        public ulong GetServerClientId()
        {
            return NetworkManager.Singleton.ConnectedClientsList[0].ClientId;
        }
        
        public PlayerNetwork GetLocalPlayerNetwork()
        {
            NetworkObject playerObject = NetworkManager.Singleton.LocalClient.PlayerObject;
            PlayerNetwork playerNetwork = playerObject.TryGetComponent<PlayerNetwork>(out var network) ? network : null;
            return playerNetwork;
        }
        
        public NetworkObject GetPlayerNetworkObjectById(ulong id)
        {
            NetworkObject playerNetworkObject = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            return playerNetworkObject;
        }
        
        public PlayerNetwork GetPlayerNetworkById(ulong id)
        {
            NetworkObject playerObject = NetworkManager.Singleton.ConnectedClients[id].PlayerObject;
            PlayerNetwork playerNetwork = playerObject.TryGetComponent<PlayerNetwork>(out var network) ? network : null;
            return playerNetwork;
        }
        
    }
}
