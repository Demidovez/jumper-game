using System;
using UnityEngine;
using WeaponSpace;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _jumpForce = 15f;

        [Header("Collision Info")] 
        [SerializeField] private Transform _groundCheckTransform;
        [SerializeField] private float _groundCheckRadius;
        [SerializeField] private LayerMask _groundLayerMask;
        
        [Header("Weapon")] 
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private Vector3 _weaponPositionOffset = new Vector3(0.5f, -0.5f, 0);
        
        private GameObject _weaponGameObject;
        private Weapon _weapon;
        
        public static Player Instance { get; private set; }
        
        private bool _isLookToRight = true;
        private bool _canDoubleJump;
        
        private Rigidbody2D _rigidBody;

        internal bool IsMoving { get; private set; }
        internal bool IsGrounded { get; private set; }
        internal bool IsDead { get; set; }
        
        public delegate void OnPlayerCollision(Collision2D other);
        public static event OnPlayerCollision OnPlayerCollisionEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();

            SetWeapon();
        }

        private void Update()
        {
            IsMoving = _rigidBody.velocity.x != 0;
            
            SetLookDirection();
            CollisionCheck();
        }

        private void SetWeapon()
        {
            _weaponGameObject = Instantiate(_weaponPrefab, transform.position + _weaponPositionOffset, Quaternion.identity, transform);
            _weapon = _weaponGameObject.GetComponent<Weapon>();
        }

        internal void Move(float moveValue)
        {
            if (IsDead)
            {
                return;
            }
            
            _rigidBody.velocity = new Vector2(moveValue * _speed, _rigidBody.velocity.y);
        }

        internal void Jump()
        {
            if (!IsDead && (IsGrounded || _canDoubleJump))
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
                _canDoubleJump = IsGrounded;  
            }
        }
        
        public void Damage(bool canFire)
        {
            _weapon.IsFiring = canFire;
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

        // public void Restore()
        // {
        //     IsDead = false;
        // }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnPlayerCollisionEvent?.Invoke(other);
        }

        // Use for setting
        // private void OnDrawGizmos()
        // {
        //     Gizmos.DrawWireSphere(_groundCheckTransform.position, _groundCheckRadius);
        // }
    }
}

