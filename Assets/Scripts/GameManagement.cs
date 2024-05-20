using EnemySpace;
using PlayerSpace;
using TMPro;
using UnityEngine;


namespace GameManagementSpace
{
    public class GameManagement : MonoBehaviour
    {
        [Header("Characters")]
        [SerializeField] private Player _player;
        [SerializeField] private Enemy _enemy;

        [Header("Fruits")]
        [SerializeField] private GameObject _fruitApple;
        [SerializeField] private GameObject _fruitBanana;
        [SerializeField] private GameObject _fruitMelon;
        
        [Header("Stats")]
        [SerializeField] private TMP_Text _textCountFruits;
        [SerializeField] private TMP_Text _textCountKilled;

        [Header("UI")] 
        [SerializeField] private Canvas _popupsContainer;
        [SerializeField] private GameObject _popupGameOver;
        
        [Header("Other")]
        [SerializeField] private GameObject _checkPoint;
        
        private int _allFruitsCollectedCount = 0;
        private int _allKilledEnemiesCount = 0;
        
        private void OnEnable()
        {
            Enemy.OnEnemyDieEvent += OnEnemyDie;
            Enemy.OnEnemyDamageEvent += OnEnemyDamage;
            Player.OnPlayerCollisionEvent += OnPlayerCollision;
        }

        private void Start()
        {
            ResetStats();
        }
        
        public void ClickNewGame()
        {
            ReloadObjects();
            ResetStats();
            
            _player.Restore();
        }

        private void ResetStats()
        {
            _textCountFruits.text = "Собрано фруктов: 0";
            _textCountKilled.text = "Убито врагов: 0";

            _allFruitsCollectedCount = 0;
            _allKilledEnemiesCount = 0;
        }

        private void TryShowCheckpoint()
        {
            if (_enemy.IsDead && !_fruitApple.activeSelf && !_fruitBanana.activeSelf && !_fruitMelon.activeSelf)
            {
                _checkPoint.SetActive(true);
            }
        }
        
        private void OnEnemyDie()
        {
            _allKilledEnemiesCount += 1;
            UpdateStats();
            TryShowCheckpoint();
        }
        
        private void OnEnemyDamage()
        {
            _enemy.IsKilledSomeone = true;
            _player.IsDead = true;

            GameOver();
        }

        private void GameOver()
        {
            _textCountFruits.text = "";
            _textCountKilled.text = "";

            Time.timeScale = 0f;
            Instantiate(_popupGameOver, Vector3.zero, Quaternion.identity, _popupsContainer.transform);
        }

        private void OnPlayerCollision(Collision2D other)
        {
            if (other.gameObject.CompareTag("Fruit"))
            {
                other.gameObject.SetActive(false);

                _allFruitsCollectedCount += 1;

                UpdateStats();
                TryShowCheckpoint();
            } else if (other.gameObject.CompareTag("Checkpoint"))
            {
                ReloadObjects();
            }
        }

        private void UpdateStats()
        {
            _textCountFruits.text = $"Собрано фруктов: {_allFruitsCollectedCount}";
            _textCountKilled.text = $"Убито врагов: {_allKilledEnemiesCount}";
        }

        private void ReloadObjects()
        {
            _checkPoint.SetActive(false);
            
            _fruitApple.SetActive(true);
            _fruitBanana.SetActive(true);
            _fruitMelon.SetActive(true);

            _enemy.Restore();
        }
        
        private void OnDisable()
        {
            Enemy.OnEnemyDieEvent -= OnEnemyDie;
            Enemy.OnEnemyDamageEvent += OnEnemyDamage;
            Player.OnPlayerCollisionEvent -= OnPlayerCollision;
        }
    }
}