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
        
        private GameObject _weaponGameObject;
        private Weapon _weapon;
        
        public static Player Instance { get; private set; }
        
        private bool _isLookToRight = true;
        private bool _canDoubleJump;
        private ContactFilter2D _groundCheckFilter;
        
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
            _groundCheckFilter.SetLayerMask(_groundLayerMask);
            
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
            IsGrounded = _groundCheckCollider.OverlapCollider(_groundCheckFilter, new Collider2D[1]) > 0;
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
    }
}

