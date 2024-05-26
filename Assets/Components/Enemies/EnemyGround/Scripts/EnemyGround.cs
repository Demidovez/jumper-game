using UnityEngine;

namespace EnemySpace
{
    public class EnemyGround : Enemy
    {
        [SerializeField] protected float _moveValue = -1f;
        
        [Header("Collision Info")] 
        [SerializeField] private Transform _barrierCheckTransform;
        [SerializeField] private float _barrierCheckRadius;
        [SerializeField] private LayerMask _barrierLayerMask;
        
        private Rigidbody2D _rigidBody;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }
        
        private void Update()
        {
            Movement();
            ReachedBarrierCheck();
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
        
        protected override void Movement()
        {
            _rigidBody.velocity = new Vector2(_moveValue * MoveValue * Speed * Time.deltaTime, _rigidBody.velocity.y);
        }
    }
}

