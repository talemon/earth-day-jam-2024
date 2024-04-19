using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace MenuScripts
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameStateManager gameStateManager;
        
        [SerializeField] private Button playButton;
        [SerializeField] private Button creditsButton;
        [SerializeField] private Button exitButton;

        [SerializeField] private SceneAsset playScene;
    
        private void Start()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            creditsButton.onClick.AddListener(OnCreditsClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            
            Debug.Log(gameStateManager.GetGameState().ShipName);
        }

        private void OnPlayClicked()
        {
            gameStateManager.Initialize();
            SceneManager.LoadScene(playScene.name);
        }

        private void OnCreditsClicked()
        {
        
        }

        private void OnExitClicked()
        {
            Application.Quit();
        }
    }
}