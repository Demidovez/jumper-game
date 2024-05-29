using UnityEngine;

namespace EnemySpace
{
    public class EnemyFly : Enemy
    {
        [SerializeField] private Transform _rightMovePosition;
        [SerializeField] private Transform _leftMovePosition;
        
        private Vector3 _rightPosition;
        private Vector3 _leftPosition;
        private bool _isMovingToLeft;
        
        private Rigidbody2D _rigidBody;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _rightPosition = _rightMovePosition.position;
            _leftPosition = _leftMovePosition.position;
            
            transform.position = _rightPosition;
            _isMovingToLeft = true;
        }
        
        private void Update()
        {
            TargetPositionCheck();
        }
        
        private void FixedUpdate()
        {
            Movement();
        }

        private void TargetPositionCheck()
        {
            if (transform.position.x <= _leftPosition.x && _isMovingToLeft)
            {
                _isMovingToLeft = false;
                transform.Rotate(0, 180, 0);
                return;
            }
            
            if (transform.position.x >= _rightPosition.x && !_isMovingToLeft)
            {
                _isMovingToLeft = true;
                transform.Rotate(0, 180, 0);
            }
        }
        
        protected override void Movement()
        {
            var direction = _isMovingToLeft ? -1 : 1;
            
            _rigidBody.velocity = new Vector2( direction * MoveValue * Speed * Time.deltaTime, _rigidBody.velocity.y);
        }
    }
}

