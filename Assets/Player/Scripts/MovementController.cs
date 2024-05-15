using UnityEngine;

namespace PlayerSpace
{
    public class MovementController : MonoBehaviour
    {
        private Player _player;
        private float _moveInput;

        private void Start()
        {
            _player = GetComponent<Player>();
        }

        private void Update()
        {
            _moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                _player.Jump();
            }
        }

        private void FixedUpdate()
        {
            _player.Move(_moveInput);
        }
    }
}

