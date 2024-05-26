using TagInterfacesSpace;
using UnityEngine;

namespace WeaponSpace
{
    public abstract class Weapon: MonoBehaviour, IWeapon
    {
        internal bool IsFiring { get; set; }

        public abstract string GetName();
    }
}