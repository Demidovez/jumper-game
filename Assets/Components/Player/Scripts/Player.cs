using GameManagementSpace;
using UnityEngine;
using WeaponSpace;
using DG.Tweening;

namespace PlayerSpace
{
    public class Player : MonoBehaviour
    {
        [Header("Health")] 
        [SerializeField] private int _allCountLives = 5;
        
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
        private int _countLives;
        private bool _isDead;

        public delegate void OnPlayerCollision(GameObject other);
        public static event OnPlayerCollision OnPlayerCollisionEvent;
        
        public delegate void OnPlayerDie();
        public static event OnPlayerDie OnPlayerDieEvent;

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

            _countLives = _allCountLives;
            
            SetWeapon();
        }

        private void Update()
        {
            CollisionCheck();
            
            _animator.SetBool("isGrounded",  _isGrounded);
            _animator.SetFloat("velocityY",  _rigidBody.velocity.y);
            _animator.SetBool("isDead",  _isDead);

            CheckBounds();
        }

        private void SetWeapon()
        {
            _weaponGameObject = Instantiate(_weaponPrefab, transform.position + _weaponPositionOffset, Quaternion.identity, transform);
            _weapon = _weaponGameObject.GetComponent<Weapon>();
        }

        internal void Move(float moveValue)
        {
            if (_isDead)
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
            if (!_isDead && (_isGrounded || _canDoubleJump))
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

        private void LoseLive()
        {
            _countLives--;
            
            _rigidBody.velocity = new Vector2(_rigidBody.velocity.x, _jumpForce);

            var spriteRenderer = GetComponent<SpriteRenderer>();

            DOTween.Sequence()
                .Append(spriteRenderer.DOColor(Color.red, 0.1f))
                .Append(spriteRenderer.DOColor(Color.white, 0.1f))
                .Append(spriteRenderer.DOColor(Color.red, 0.1f))
                .Append(spriteRenderer.DOColor(Color.white, 0.1f));
        }

        public void TakeDamage()
        {
            if (_countLives > 0)
            {
                LoseLive();
                return;
            } 
            
            _isDead = true;
            OnPlayerDieEvent?.Invoke();
        }

        private void CollisionCheck()
        {
            _isGrounded = _groundCheckCollider.OverlapCollider(_groundCheckFilter, new Collider2D[1]) > 0;
        }
        
        private void OnCollisionEnter2D(Collision2D other)
        {
            OnPlayerCollisionEvent?.Invoke(other.gameObject);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            OnPlayerCollisionEvent?.Invoke(other.gameObject);
        }
    }
}

