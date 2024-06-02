using System;
using UnityEngine;

namespace GameManagementSpace
{
    public class Global: MonoBehaviour
    {
        [SerializeField] private BoxCollider2D _levelBounds;

        public static Global Instance { get; private set; }
        public Bounds LevelBounds => _levelBounds.bounds;

        private void Awake()
        {
            Instance = this;
        }
    }
}