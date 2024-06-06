using BulletSpace;
using EnemySpace;
using PlayerSpace;
using PopupSpace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;
using TrapSpace;
using WeaponSpace;

namespace GameManagementSpace
{
    public class GameManagement : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private TMP_Text _textCountFruits;
        [SerializeField] private TMP_Text _textCountKilled;
        
        [Header("Other")]
        [SerializeField] private GameObject _checkPoint;
        
        private int _allFruitsCollectedCount;
        private int _allKilledEnemiesCount;
        
        private void OnEnable()
        {
            Enemy.OnEnemyDieAreaCollisionEvent += OnEnemyDieAreaCollision;
            Enemy.OnEnemyDamageAreaCollisionEvent += OnEnemyDamageAreaCollision;
            Enemy.OnEnemyDieEvent += OnEnemyDie;
            Fruit.OnFruitCollisionEvent += OnFruitCollision;
            Fire.OnFireCollisionEvent += OnFireCollision;
            LevelPoint.OnLevelPointCollisionEvent += OnLevelPointCollision;
            Player.OnPlayerDieEvent += OnPlayerDie;
            Bullet.OnBulletCollisionEvent += OnBulletCollision;
            PopupsManagement.OnPopupNewGameEvent += OnStartNewGame;
            Checkpoint.OnCheckpointCollisionEvent += OnCheckpointCollision;
        }
        
        private void Start()
        {
            ResetStats();
        }

        private static void OnEnemyDieAreaCollision(Enemy enemy, GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                enemy.TakeDamage();
            }
        }
        
        private static void OnEnemyDamageAreaCollision(GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                Player.Instance.TakeDamage();
            }
        }
        
        private void OnEnemyDie()
        {
            _allKilledEnemiesCount += 1;
            UpdateStats();
        }
        
        private void OnFruitCollision(GameObject obj, GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                obj.SetActive(false);

                _allFruitsCollectedCount += 1;

                UpdateStats();
            }
        }
        
        private static void OnFireCollision(GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                Player.Instance.TakeDamage();
                return;
            }

            if (other.TryGetComponent(out Box box))
            {
                box.DestroyObject();
            }
        }
        
        private static void OnLevelPointCollision(GameObject other)
        {
            DOTween.Clear(true);
            SceneManager.LoadScene("Level 2");
        }
        
        private static void OnBulletCollision(Bullet bullet, GameObject other)
        {
            if (other.TryGetComponent(out Enemy enemy))
            {
                bullet.SetInactive();
                enemy.TakeDamage();
                
                return;
            }
            
            if (other.TryGetComponent(out Box box))
            {
                bullet.SetInactive();
                box.DestroyObject();
                
                return;
            }

            if (other.GetComponent<Fruit>())
            {
                return;
            }
            
            bullet.SetInactive();
        }

        private void OnCheckpointCollision(GameObject other)
        {
            if (other == Player.Instance.gameObject)
            {
                _textCountFruits.text = "";
                _textCountKilled.text = "";

                PopupsManagement.Instance.ShowGameWinPopup();
            }
        }
        
        private static void OnStartNewGame()
        {
            PopupsManagement.Instance.HidePopups();

            DOTween.Clear(true);
            SceneManager.LoadScene("Level 1");
        }

        private void ResetStats()
        {
            _textCountFruits.text = "Собрано фруктов: 0";
            _textCountKilled.text = "Убито врагов: 0";

            _allFruitsCollectedCount = 0;
            _allKilledEnemiesCount = 0;
        }

        private void OnPlayerDie()
        {
            _textCountFruits.text = "";
            _textCountKilled.text = "";
            
            PopupsManagement.Instance.ShowGameOverPopup();
        }

        private void UpdateStats()
        {
            _textCountFruits.text = $"Собрано фруктов: {_allFruitsCollectedCount}";
            _textCountKilled.text = $"Убито врагов: {_allKilledEnemiesCount}";
        }
        
        private void OnDisable()
        {
            Enemy.OnEnemyDieAreaCollisionEvent -= OnEnemyDieAreaCollision;
            Enemy.OnEnemyDamageAreaCollisionEvent -= OnEnemyDamageAreaCollision;
            Enemy.OnEnemyDieEvent -= OnEnemyDie;
            Fruit.OnFruitCollisionEvent -= OnFruitCollision;
            Fire.OnFireCollisionEvent -= OnFireCollision;
            LevelPoint.OnLevelPointCollisionEvent -= OnLevelPointCollision;
            Player.OnPlayerDieEvent -= OnPlayerDie;
            Bullet.OnBulletCollisionEvent -= OnBulletCollision;
            PopupsManagement.OnPopupNewGameEvent -= OnStartNewGame;
            Checkpoint.OnCheckpointCollisionEvent -= OnCheckpointCollision;
        }
    }
}