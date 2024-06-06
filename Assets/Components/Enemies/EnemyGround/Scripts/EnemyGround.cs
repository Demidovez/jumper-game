using UnityEngine;

namespace EnemySpace
{
    public class EnemyGround : Enemy
    {
        [Header("Collision Info")] 
        [SerializeField] private Transform _barrierCheckTransform;
        [SerializeField] private float _barrierCheckRadius;
        [SerializeField] private LayerMask _barrierLayerMask;
        
        private Rigidbody2D _rigidBody;
        private int _direction = -1;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            ReachedBarrierCheck();
        }
        
        private void FixedUpdate()
        {
            Movement();
        }
        
        private void ReachedBarrierCheck()
        {
            bool isReachedBarrier = Physics2D.OverlapCircle(_barrierCheckTransform.position, _barrierCheckRadius, _barrierLayerMask);

            if (isReachedBarrier)
            {
                _direction *= -1;
                transform.Rotate(0, 180, 0);
            }
        }
        
        protected override void Movement()
        {
            _rigidBody.velocity = new Vector2(_direction * MoveValue * Speed * Time.deltaTime, _rigidBody.velocity.y);
        }
    }
}

