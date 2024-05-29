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
        public delegate void OnEnemyDamage();
        public static event OnEnemyDie OnEnemyDieEvent;
        public static event OnEnemyDamage OnEnemyDamageEvent;
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            ITag tagInstance = other.gameObject.GetComponent<ITag>();
            
            if (tagInstance is IPlayer)
            {
                if (other.collider.IsTouching(_deathCollider))
                {
                    gameObject.SetActive(false);
                    OnEnemyDieEvent?.Invoke();
                }
                else if(other.collider.IsTouching(_damageCollider))
                {
                    OnEnemyDamageEvent?.Invoke();
                }
            }
        }

        protected abstract void Movement();
        
        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }
    }
}