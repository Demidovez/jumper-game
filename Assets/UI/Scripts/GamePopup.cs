using UnityEngine;
using UnityEngine.UI;

namespace UISpace
{
    public class GamePopup: MonoBehaviour
    {
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _exitButton;
        
        public delegate void OnGamePopupNewGame();
        public static event OnGamePopupNewGame OnGamePopupNewGameEvent;

        private void Start()
        {
            _restartButton.onClick.AddListener(() => OnGamePopupNewGameEvent?.Invoke());
            _exitButton.onClick.AddListener(QuitGame);
        }

        private void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #endif
            Application.Quit();
        }
    }
}