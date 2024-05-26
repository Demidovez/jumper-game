using TagInterfacesSpace;
using UnityEngine;

namespace BulletSpace
{
    public abstract class Bullet : MonoBehaviour
    {
        [SerializeField] private float _moveForce = 75f;
        [SerializeField] private float _liveTime = 1f;
        
        private Rigidbody2D _rigidBody;
        private bool _isMoved;
        private float _timerToInactive;

        private void Start()
        {
            _rigidBody = GetComponent<Rigidbody2D>();
        }

        private void Update()
        {
            if (_isMoved)
            {
                _timerToInactive += Time.deltaTime;

                if (_timerToInactive >= _liveTime)
                {
                    SetInactive();
                }
            }
            else
            {
                float direction = transform.rotation.y >= 0 ? 1 : -1;
                
                _rigidBody.velocity = new Vector2(direction * _moveForce, _rigidBody.velocity.y);
                _isMoved = true;
            }
        }

        private void SetInactive()
        {
            _timerToInactive = 0f;
            _isMoved = false;
            gameObject.SetActive(false);
        }

        private void OnCollisionEnter2D(Collision2D other)
        {
            other.gameObject.TryGetComponent(out ITag tagInstance);
            
            switch (tagInstance)
            {
                case IWeapon:
                case IPlayer:
                    return;
                case IDestructible destructible:
                    destructible.DestroyObject();
                    break;
            }

            SetInactive();
        }
    }
}

