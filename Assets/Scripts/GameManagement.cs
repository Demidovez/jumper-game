using EnemySpace;
using PlayerSpace;
using TMPro;
using UISpace;
using UnityEngine;
using UnityEngine.SceneManagement;


namespace GameManagementSpace
{
    public class GameManagement : MonoBehaviour
    {
        [Header("Characters")]
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
        [SerializeField] private GameObject _popupGameOverPrefab;
        [SerializeField] private GameObject _popupGameWinPrefab;
        
        [Header("Other")]
        [SerializeField] private GameObject _checkPoint;
        
        private int _allFruitsCollectedCount = 0;
        private int _allKilledEnemiesCount = 0;

        private GameObject _popupGameOver;
        private GameObject _popupGameWin;
        
        private void OnEnable()
        {
            Enemy.OnEnemyDieEvent += OnEnemyDie;
            Enemy.OnEnemyDamageEvent += OnEnemyDamage;
            Player.OnPlayerCollisionEvent += OnPlayerCollision;
            GamePopup.OnGamePopupNewGameEvent += OnStartNewGame;
        }

        private void Start()
        {
            ResetStats();
        }
        
        private void OnStartNewGame()
        {
            Time.timeScale = 1f;
            Destroy(_popupGameOver);
            Destroy(_popupGameWin);

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
            Player.Instance.IsDead = true;

            GameOver();
        }

        private void GameOver()
        {
            _textCountFruits.text = "";
            _textCountKilled.text = "";

            Time.timeScale = 0f;
            
            _popupGameOver = Instantiate(_popupGameOverPrefab, _popupsContainer.transform.position, Quaternion.identity, _popupsContainer.transform);
        }
        
        private void GameWin()
        {
            _textCountFruits.text = "";
            _textCountKilled.text = "";

            Time.timeScale = 0f;
            
            _popupGameOver = Instantiate(_popupGameWinPrefab, _popupsContainer.transform.position, Quaternion.identity, _popupsContainer.transform);
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
                GameWin();
            }
        }

        private void UpdateStats()
        {
            _textCountFruits.text = $"Собрано фруктов: {_allFruitsCollectedCount}";
            _textCountKilled.text = $"Убито врагов: {_allKilledEnemiesCount}";
        }
        
        private void OnDisable()
        {
            Enemy.OnEnemyDieEvent -= OnEnemyDie;
            Enemy.OnEnemyDamageEvent -= OnEnemyDamage;
            Player.OnPlayerCollisionEvent -= OnPlayerCollision;
            GamePopup.OnGamePopupNewGameEvent -= OnStartNewGame;
        }
    }
}