using System;
using UnityEngine;

// зачем sorting layers в tilemap?

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 1f;
        [SerializeField] private float _jumpForce = 5f;

        [Header("Collision Info")] 
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private LayerMask _groundLayerMask;

        private bool _isLookToRight;
        private bool _canDoubleJump;
        
        private Rigidbody2D _rigidBody;

        internal bool IsMoving { get; private set; }
        internal bool IsGrounded { get; private set; }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            
            IsMoving = false;
            _isLookToRight = true;
            _canDoubleJump = false;
        }

        private void Update()
        {
            IsMoving = _rigidBody.velocity.x != 0;
            
            SetLookDirection();
            CollisionCheck();
        }

        internal void Move(float moveValue)
        {
            _rigidBody.velocity = new Vector2(moveValue * _speed, _rigidBody.velocity.y);
        }

        internal void Jump()
        {
            if (IsGrounded || _canDoubleJump)
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
                _canDoubleJump = IsGrounded;  
            }
        }

        private void SetLookDirection()
        {
            if (IsMoving && (_isLookToRight != _rigidBody.velocity.x > 0))
            {
                _isLookToRight = !_isLookToRight;
                transform.Rotate(0, 180, 0);
            }
        }

        private void CollisionCheck()
        {
            IsGrounded = Physics2D.OverlapCircle(_groundCheckTransform.position, _groundCheckRadius, _groundLayerMask);
        }

        public float GetVelocityY()
        {
            return _rigidBody.velocity.y;
        }

        // Use for setting
        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireSphere(_groundCheckTransform.position, _groundCheckRadius);
        // }
        
        // private void OnCollisionEnter2D(Collision2D other)
        // {
        //     Debug.Log(222);
        // }
    }
}

