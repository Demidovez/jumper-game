using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSpace
{
    public abstract class Weapon: MonoBehaviour
    {
        internal bool IsFiring { get; set; }

        public abstract string GetName();
    }
}