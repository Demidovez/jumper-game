using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameManagementSpace
{
    public class SceneData : MonoBehaviour
    {
        public static SceneData Instance { get; private set; }
        
        private int _allFruitsCollectedCount;
        private int _allKilledEnemiesCount;
        private int _playerLives;
        
        public delegate void OnSceneDataRestored(int fruits, int enemies, int lives);
        public static event OnSceneDataRestored OnSceneDataRestoredEvent;
        
        private void Awake()
        {
            Instance = this;
            DontDestroyOnLoad (this);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        
        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            OnSceneDataRestoredEvent?.Invoke(_allFruitsCollectedCount, _allKilledEnemiesCount, _playerLives);
        }

        public void Save(int fruits, int enemies, int lives)
        {
            _allFruitsCollectedCount = fruits;
            _allKilledEnemiesCount = enemies;
            _playerLives = lives;
        }

        private void OnDisable ()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}

