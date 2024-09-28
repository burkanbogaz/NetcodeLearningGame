using Unity.Netcode;
using UnityEngine;

namespace _Scripts
{
    public class GameManager : MonoBehaviour
    {
        //singleton
        public static GameManager Instance { get; private set; }
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
        
        [SerializeField] private GameObject objectToSpawn;
        
        public void SpawnObjects()
        {
            for (int i = 0; i < 10; i++)
            {
                Vector3 randomPosition = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                GameObject spawnedObject = Instantiate(objectToSpawn, randomPosition, Quaternion.identity);
                NetworkObject networkObject = spawnedObject.GetComponent<NetworkObject>();
                networkObject.Spawn();
            }
        }
        
    
    }
}
