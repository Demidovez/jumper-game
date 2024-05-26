using UnityEngine;

namespace PlayerSpace
{
    public class MovementController : MonoBehaviour
    {
        private float _moveInput;

        private void Update()
        {
            _moveInput = Input.GetAxisRaw("Horizontal");

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Player.Instance.Jump();
            }
        }

        private void FixedUpdate()
        {
            Player.Instance.Move(_moveInput);
        }
    }
}

