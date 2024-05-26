using UnityEngine;

namespace EnemySpace
{
    public class EnemyFly : Enemy
    {
        [SerializeField] private Transform _startMovePosition;
        [SerializeField] private Transform _endMovePosition;
        
        private Vector3 _startPosition;
        private Vector3 _endPosition;
        private bool _isMovingToEnd;
        
        private Rigidbody2D _rigidBody;
        
        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _startPosition = _startMovePosition.position;
            _endPosition = _endMovePosition.position;
            
            transform.position = _startPosition;
            _isMovingToEnd = true;
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
            if (transform.position.x >= _endPosition.x && _isMovingToEnd)
            {
                _isMovingToEnd = false;
                transform.Rotate(0, 180, 0);
                return;
            }
            
            if (transform.position.x <= _startPosition.x && !_isMovingToEnd)
            {
                _isMovingToEnd = true;
                transform.Rotate(0, 180, 0);
            }
        }
        
        protected override void Movement()
        {
            var direction = _isMovingToEnd ? 1 : -1;
            
            _rigidBody.velocity = new Vector2( direction * MoveValue * Speed * Time.deltaTime, _rigidBody.velocity.y);
        }
    }
}

