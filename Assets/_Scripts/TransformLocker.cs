using UnityEngine;

namespace _Scripts
{
    public class TransformLocker : MonoBehaviour
    {
        private Transform _lockedTransform;

        private void LateUpdate()
        {
            if(_lockedTransform == null) return;
            
            transform.position = _lockedTransform.position;
            transform.rotation = _lockedTransform.rotation;
        }
        
        public void Lock(Transform transformToLock)
        {
            _lockedTransform = transformToLock;
        }
        
        public void Unlock()
        {
            _lockedTransform = null;
        }
    }
}
