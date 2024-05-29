using UnityEngine;

namespace EnemySpace
{
    public class EnemyCliff : Enemy
    {
        [Header("Collision Info")] 
        [SerializeField] private Collider2D _cliffCheckCollider;
        [SerializeField] private LayerMask _groundLayerMask;
        
        private int _moveDirection = -1;
        private ContactFilter2D _groundCheckFilter;
        private Rigidbody2D _rigidBody;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _groundCheckFilter.SetLayerMask(_groundLayerMask);
        }
        
        private void Update()
        {
            CliffCheck();
        }
        
        private void FixedUpdate()
        {
            Movement();
        }

        private void CliffCheck()
        {
            bool hasGround = _cliffCheckCollider.OverlapCollider(_groundCheckFilter, new Collider2D[1]) > 0;

            if (!hasGround)
            {
                _moveDirection *= -1;
                transform.Rotate(0, 180, 0);
            }
        }
        
        protected override void Movement()
        {
            _rigidBody.velocity = new Vector2( _moveDirection * MoveValue * Speed * Time.deltaTime, _rigidBody.velocity.y);
        }
    }
}

