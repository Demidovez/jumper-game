using System;
using GameManagementSpace;
using TagInterfacesSpace;
using UnityEngine;
using WeaponSpace;

namespace PlayerSpace
{
    public class Player : MonoBehaviour, IPlayer
    {
        [Header("Movement")]
        [SerializeField] private float _speed = 10f;
        [SerializeField] private float _jumpForce = 15f;

        [Header("Collision Info")] 
        [SerializeField] private Collider2D _groundCheckCollider;
        [SerializeField] private LayerMask _groundLayerMask;
        
        [Header("Weapon")] 
        [SerializeField] private GameObject _weaponPrefab;
        [SerializeField] private Vector3 _weaponPositionOffset = new Vector3(0.5f, -0.5f, 0);
        
        private Animator _animator;
        private GameObject _weaponGameObject;
        private Weapon _weapon;
        
        public static Player Instance { get; private set; }
        
        private bool _isLookToRight = true;
        private bool _canDoubleJump;
        private ContactFilter2D _groundCheckFilter;
        private Bounds _levelBounds;
        
        private Rigidbody2D _rigidBody;
        
        private bool _isGrounded;
        internal bool IsDead { get; set; }
        
        public delegate void OnPlayerCollision(GameObject other);
        public static event OnPlayerCollision OnPlayerCollisionEvent;

        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _animator = GetComponent<Animator>();
            
            _groundCheckFilter.SetLayerMask(_groundLayerMask);
            _levelBounds = Global.Instance.LevelBounds;
            
            SetWeapon();
        }

        private void Update()
        {
            CollisionCheck();
            
            _animator.SetBool("isGrounded",  _isGrounded);
            _animator.SetFloat("velocityY",  _rigidBody.velocity.y);
            _animator.SetBool("isDead",  IsDead);

            CheckBounds();
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
            
            _animator.SetBool("isMove",  Mathf.Abs(moveValue) > 0.1f);
            
            bool shouldRotate = (moveValue > 0 && !_isLookToRight) || (moveValue < 0 && _isLookToRight);
            
            if (shouldRotate)
            {
                _isLookToRight = !_isLookToRight;
                transform.Rotate(0, 180, 0);
            }
        }

        internal void Jump()
        {
            if (!IsDead && (_isGrounded || _canDoubleJump))
            {
                _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);
                _canDoubleJump = _isGrounded;  
            }
        }

        private void CheckBounds()
        {
            transform.position = new Vector3(
                Mathf.Clamp(transform.position.x, _levelBounds.min.x, _levelBounds.max.x),
                Mathf.Clamp(transform.position.y, _levelBounds.min.y, _levelBounds.max.y),
                transform.position.z
            );
        }
        
        public void Damage(bool canFire)
        {
            _weapon.IsFiring = canFire;
        }

        private void CollisionCheck()
        {
            _isGrounded = _groundCheckCollider.OverlapCollider(_groundCheckFilter, new Collider2D[1]) > 0;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnPlayerCollisionEvent?.Invoke(other.gameObject);
        }
    }
}

