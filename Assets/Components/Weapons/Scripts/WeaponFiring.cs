using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSpace
{
    public class WeaponFiring: MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private GameObject _bulletsPrefab;
        [SerializeField] private Transform _bulletStartPosition;
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private float _spawnInterval = 0.5f;

        private List<GameObject> _bulletsPool;
        private GameObject _bullets;
        private Weapon _weapon;
        private float _timer;
        
        private void Start()
        {
            _bullets = Instantiate(_bulletsPrefab);
            _weapon = GetComponent<Weapon>();
            
            BulletsInit();
        }

        private void Update()
        {
            if (_weapon.IsFiring)
            {
                _timer += Time.deltaTime;

                if (_timer >= _spawnInterval)
                {
                    _timer = 0;
                    SpawnBullet();
                }
            }
            else
            {
                _timer = _spawnInterval;
            }
        }

        private void SpawnBullet()
        {
            GameObject bullet = GetBullet();

            if (bullet)
            {
                bullet.transform.position = _bulletStartPosition.position;
                bullet.transform.rotation = _bulletStartPosition.rotation;
                bullet.SetActive(true);
            }
        }

        private void BulletsInit()
        {
            _bulletsPool = new List<GameObject>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                CreateBullet();
            }
        }

        private GameObject GetBullet()
        {
            foreach (var bullet in _bulletsPool)
            {
                if (!bullet.activeInHierarchy)
                {
                    return bullet;
                }
            }

            return CreateBullet();
        }

        private GameObject CreateBullet()
        {
            GameObject newBullet = Instantiate(_bulletPrefab, _bullets.transform);
            newBullet.SetActive(false);
                
            _bulletsPool.Add(newBullet);

            return newBullet;
        }
    }
}