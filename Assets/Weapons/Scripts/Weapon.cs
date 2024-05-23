using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSpace
{
    public abstract class Weapon: MonoBehaviour
    {
        internal bool IsFiring { get; set; }

        public void SetDirection(Vector3 targetPosition)
        {
            transform.rotation = Quaternion.FromToRotation(transform.position, targetPosition);
        }

        public abstract string GetName();
    }
}