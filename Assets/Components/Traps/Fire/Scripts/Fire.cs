using TagInterfacesSpace;
using UnityEngine;

namespace TrapSpace
{
    public class Fire : MonoBehaviour, ITrap
    {
        private void OnCollisionEnter2D(Collision2D other)
        {
            ITag tagInstance = other.gameObject.GetComponent<ITag>();

            if (tagInstance is IDestructible destructible)
            {
                destructible.DestroyObject();
            }
        }
    }
}
