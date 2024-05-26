using TagInterfacesSpace;
using UnityEngine;

namespace EnemySpace
{
    public class Enemy: MonoBehaviour, IDestructible, IEnemy
    {
        [SerializeField] private float _speed = 5f;
        [SerializeField] private float _moveValue = -1f;
        
        [Header("Collision Info")] 
        [SerializeField] private Transform _barrierCheckTransform;
        [SerializeField] private float _barrierCheckRadius;
        [SerializeField] private LayerMask _barrierLayerMask;
        
        [SerializeField] private BoxCollider2D _deathCollider;
        [SerializeField] private BoxCollider2D _damageCollider;

        public delegate void OnEnemyDie();
        public static event OnEnemyDie OnEnemyDieEvent;
        
        public delegate void OnEnemyDamage();
        public static event OnEnemyDamage OnEnemyDamageEvent;
        
        private Rigidbody2D _rigidBody;
        private Vector3 _startPosition;
        
        public bool IsDead { get; private set; }
        public bool IsKilledSomeone { get; set; }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            
            IsDead = false;
            IsKilledSomeone = false;
            _startPosition = transform.position;
        }

        private void Update()
        {
            Movement();
            ReachedBarrierCheck();
        }

        public void Restore()
        {
            IsDead = false;
            IsKilledSomeone = false;
            gameObject.SetActive(true);
            
            transform.position = _startPosition;

            if (_moveValue > 0)
            {
                transform.Rotate(0, 180, 0);
            }
            
            _moveValue = -1f;
        }
        
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
                    Damage();
                }
            }
        }

        private void Die()
        {
            gameObject.SetActive(false);
            IsDead = true;
            
            OnEnemyDieEvent?.Invoke();
        }
        
        private void Damage()
        {
            OnEnemyDamageEvent?.Invoke();
        }

        private void ReachedBarrierCheck()
        {
            bool isReachedBarrier = Physics2D.OverlapCircle(_barrierCheckTransform.position, _barrierCheckRadius, _barrierLayerMask);

            if (isReachedBarrier)
            {
                _moveValue *= -1;
                transform.Rotate(0, 180, 0);
            }
        }
        
        private void Movement()
        {
            if (!IsKilledSomeone)
            {
                _rigidBody.velocity = new Vector2(_moveValue * _speed, _rigidBody.velocity.y);
            }
        }
        
        public void DestroyObject()
        {
            gameObject.SetActive(false);
        }
    }
}