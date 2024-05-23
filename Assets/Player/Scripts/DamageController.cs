using UnityEngine;

namespace PlayerSpace
{
    public class DamageController : MonoBehaviour
    {
        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                Player.Instance.Damage(true);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Player.Instance.Damage(false);
            }

            Player.Instance.SetDamageDirection(Input.mousePosition);
        }
    }
}

