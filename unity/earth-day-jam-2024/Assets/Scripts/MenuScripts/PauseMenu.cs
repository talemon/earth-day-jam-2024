using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuScripts
{
    public class PauseMenu : MonoBehaviour
    {
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
            if (Input.GetButtonUp("Cancel"))
            {
                IsVisible = !IsVisible;
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
        }
        
        public void OnBackSelected()
        {
            IsVisible = false;
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