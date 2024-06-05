using BulletSpace;
using EnemySpace;
using PlayerSpace;
using PopupSpace;
using TagInterfacesSpace;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagementSpace
{
    public class GameManagement : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private TMP_Text _textCountFruits;
        [SerializeField] private TMP_Text _textCountKilled;
        
        [Header("Other")]
        [SerializeField] private GameObject _checkPoint;
        
        private int _allFruitsCollectedCount = 0;
        private int _allKilledEnemiesCount = 0;
        
        private void OnEnable()
        {
            Enemy.OnEnemyDieEvent += OnEnemyDie;
            Enemy.OnEnemyDamageEvent += OnEnemyDamage;
            Player.OnPlayerCollisionEvent += OnPlayerCollision;
            Bullet.OnBulletDestroyEvent += OnBulletDestroy;
            PopupsManagement.OnPopupNewGameEvent += OnStartNewGame;
        }

        private void Start()
        {
            ResetStats();
        }
        
        private void OnStartNewGame()
        {
            PopupsManagement.Instance.HidePopups();

            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        private void OnBulletDestroy(ITag obj)
        {
            if (obj is IDestructible destructible)
            {
                destructible.DestroyObject();
            }

            if (obj is IEnemy)
            {
                _allKilledEnemiesCount += 1;
                UpdateStats();
            }
        }

        private void ResetStats()
        {
            _textCountFruits.text = "Собрано фруктов: 0";
            _textCountKilled.text = "Убито врагов: 0";

            _allFruitsCollectedCount = 0;
            _allKilledEnemiesCount = 0;
        }
        
        private void OnEnemyDie()
        {
            _allKilledEnemiesCount += 1;
            UpdateStats();
        }

        private void LoseLiveOrGameOver()
        {
            if (Player.Instance.HasLive)
            {
                Player.Instance.LoseLive();
                return;
            } 
            
            Player.Instance.IsDead = true;
            
            _textCountFruits.text = "";
            _textCountKilled.text = "";
            
            PopupsManagement.Instance.ShowGameOverPopup();
        }
        
        private void PickUpFruit(GameObject fruitObj)
        {
            fruitObj.SetActive(false);

            _allFruitsCollectedCount += 1;

            UpdateStats();
        }
        
        private void GameWin()
        {
            _textCountFruits.text = "";
            _textCountKilled.text = "";

            PopupsManagement.Instance.ShowGameWinPopup();
        }

        private void OnEnemyDamage()
        {
            LoseLiveOrGameOver();
        }

        private void OnPlayerCollision(GameObject otherObject)
        {
            ITag tagInstance = otherObject.GetComponent<ITag>();
            
            switch (tagInstance)
            {
                case IFruit:
                    PickUpFruit(otherObject);
                    break;
                case ITrap:
                    LoseLiveOrGameOver();
                    break;
                case ICheckpoint:
                    GameWin();
                    break;
                case ILevelPoint:
                    NextLevel();
                    break;
            }
        }

        private void NextLevel()
        {
            SceneManager.LoadScene("Level 2");
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
            Bullet.OnBulletDestroyEvent -= OnBulletDestroy;
            PopupsManagement.OnPopupNewGameEvent -= OnStartNewGame;
        }
    }
}