using UnityEngine;

namespace GameManagementSpace
{
    public class LevelPoint: MonoBehaviour
    {
        public delegate void OnLevelPointCollision(GameObject other);
        public static event OnLevelPointCollision OnLevelPointCollisionEvent;

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnLevelPointCollisionEvent?.Invoke(other.gameObject);
        }
    }
}