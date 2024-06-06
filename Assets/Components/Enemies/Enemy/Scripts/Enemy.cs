using UnityEngine;

namespace EnemySpace
{
    public abstract class Enemy: MonoBehaviour
    {
        [SerializeField] protected float Speed = 3f;
        [SerializeField] protected float MoveValue = 100f;
        
        [SerializeField] private BoxCollider2D _deathCollider;
        [SerializeField] private BoxCollider2D _damageCollider;

        public delegate void OnEnemyDieAreaCollision(Enemy enemy, GameObject other);
        public delegate void OnEnemyDamageAreaCollision(GameObject other);
        public delegate void OnEnemyDie();
        public static event OnEnemyDieAreaCollision OnEnemyDieAreaCollisionEvent;
        public static event OnEnemyDamageAreaCollision OnEnemyDamageAreaCollisionEvent;
        public static event OnEnemyDie OnEnemyDieEvent;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            if (other.collider.IsTouching(_deathCollider))
            {
                OnEnemyDieAreaCollisionEvent?.Invoke(this, other.gameObject);
            }
            else if(other.collider.IsTouching(_damageCollider))
            {
                OnEnemyDamageAreaCollisionEvent?.Invoke(other.gameObject);
            }
        }

        protected abstract void Movement();
        
        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }

        public void TakeDamage()
        {
            OnEnemyDieEvent?.Invoke();
            DestroyObject();
        }
    }
}