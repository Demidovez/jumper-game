using TagInterfacesSpace;
using UnityEngine;

namespace EnemySpace
{
    public abstract class Enemy: MonoBehaviour, IDestructible, IEnemy
    {
        [SerializeField] protected float Speed = 3f;
        [SerializeField] protected float MoveValue = 100f;
        
        [SerializeField] private BoxCollider2D _deathCollider;
        [SerializeField] private BoxCollider2D _damageCollider;

        public delegate void OnEnemyDie();
        public static event OnEnemyDie OnEnemyDieEvent;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.TryGetComponent(out ITag tagInstance);
            
            if (tagInstance is IPlayer)
            {
                if (other.collider.IsTouching(_deathCollider))
                {
                    Die();
                }
                else if(other.collider.IsTouching(_damageCollider))
                {
                    // Damage();
                }
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
            OnEnemyDieEvent?.Invoke();
        }

        protected abstract void Movement();
        
        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }
    }
}