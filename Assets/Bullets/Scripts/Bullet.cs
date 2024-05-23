using System;
using UnityEngine;

namespace BulletSpace
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private float _moveForce = 75f;
        
        private Rigidbody2D _rigidBody;
        private bool _canMoved;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
            _canMoved = true;
        }

        private void Update()
        {
            if (gameObject.activeInHierarchy && _canMoved)
            {
                _rigidBody.velocity = new Vector2(_moveForce, _rigidBody.velocity.y);
                _canMoved = false;
            } else if (!gameObject.activeInHierarchy)
            {
                _canMoved = true;
            }
        }
    }
}

