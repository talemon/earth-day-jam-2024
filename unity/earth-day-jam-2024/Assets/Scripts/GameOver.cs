using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace MenuScripts
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private TMP_Text smallTrashText;
        [SerializeField] private TMP_Text bigTrashText;
        private bool _isVisible;

        public bool IsVisible
        {
            get => _isVisible;
            private set
            {
                if (_isVisible == value)
                    return;
                
                _isVisible = value;
                Time.timeScale = _isVisible ? 0f : 1f;
                Show(_isVisible);
            }
        }

        private void Start()
        {
            Show(false);
        }

        private void Update()
        {
            if (gameStateManager.GetGameState().GameOverTrigger && !IsVisible)
            {
                IsVisible = true;
                gameStateManager.GetGameState().GameOverTrigger = true;
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        private void Show(bool isVisible)
        {
            smallTrashText.text = gameStateManager.GetGameState().TrashCollected["SmallTrash"].ToString();
            bigTrashText.text = gameStateManager.GetGameState().TrashCollected["BigTrash"].ToString();

            foreach (Transform obj in transform)
            {
                obj.gameObject.SetActive(isVisible);
            }
        }
        
        public void OnMainMenuSelected()
        {
            SceneManager.LoadScene(0);
        }

        public void OnQuitGameSelected()
        {
            Application.Quit();
        }
    }
}