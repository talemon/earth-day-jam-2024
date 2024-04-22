using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

namespace MenuScripts
{
    public class GameOver : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        [SerializeField] private TMP_Text reportText;
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
            if (gameStateManager.GetGameState().IsGameOver && !IsVisible)
            {
                IsVisible = true;
                gameStateManager.GetGameState().IsGameOver = true;
            }
        }

        private void OnDestroy()
        {
            Time.timeScale = 1f;
        }

        private void Show(bool isVisible)
        {
            foreach (Transform obj in transform)
            {
                obj.gameObject.SetActive(isVisible);
            }

            if (isVisible)
            {
                FillReport();
            }
        }

        private void FillReport()
        {
            var state = gameStateManager.GetGameState();

            var str = new StringBuilder();
            foreach (var trashPair in state.CollectedTrash)
            {
                str.AppendLine($"{trashPair.Key.displayName}: {trashPair.Value}");
            }

            reportText.text = str.ToString();
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