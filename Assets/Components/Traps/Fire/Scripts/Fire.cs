using UnityEngine;

namespace TrapSpace
{
    public class Fire : MonoBehaviour
    {
        public delegate void OnFireCollision(GameObject other);
        public static event OnFireCollision OnFireCollisionEvent;
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            OnFireCollisionEvent?.Invoke(other.gameObject);
        }
    }
}
