using UnityEngine;

namespace EnemySpace
{
    public class Enemy: MonoBehaviour
    {
        [SerializeField] private Vector3 _startPosition = new Vector3(8.82f, -3.042f, 0f);
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
        
        public bool IsDead { get; private set; }
        public bool IsKilledSomeone { get; set; }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            
            IsDead = false;
            IsKilledSomeone = false;
            transform.position = _startPosition;
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
            if (other.gameObject.CompareTag("Alive"))
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
        
        // Use for setting
        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireSphere(_barrierCheckTransform.position, _barrierCheckRadius);
        // }
    }
}