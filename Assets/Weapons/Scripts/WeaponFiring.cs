using System;
using System.Collections.Generic;
using UnityEngine;

namespace WeaponSpace
{
    public class WeaponFiring: MonoBehaviour
    {
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private int _initialPoolSize = 20;
        [SerializeField] private float _spawnInterval = 0.5f;

        private List<GameObject> _bulletsPool;
        private Weapon _weapon;
        private float _timer;
        
        private void Start()
        {
            BulletsInit();
            _weapon = GetComponent<Weapon>();
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
                bullet.transform.position = transform.position;
                bullet.transform.rotation = transform.rotation;
                bullet.SetActive(true);
            }
        }

        private void BulletsInit()
        {
            _bulletsPool = new List<GameObject>();

            for (int i = 0; i < _initialPoolSize; i++)
            {
                GameObject bullet = Instantiate(_bulletPrefab, transform);
                bullet.SetActive(false);
                
                _bulletsPool.Add(bullet);
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
            
            GameObject newBullet = Instantiate(_bulletPrefab, transform);
            newBullet.SetActive(false);
                
            _bulletsPool.Add(newBullet);

            return newBullet;
        }
    }
}