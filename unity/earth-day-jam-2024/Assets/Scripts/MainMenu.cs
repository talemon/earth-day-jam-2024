using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button creditsButton;
    [SerializeField] private Button exitButton;

    [SerializeField] private SceneAsset playScene;
    
    private void Start()
    {
        playButton.onClick.AddListener(OnPlayClicked);
        creditsButton.onClick.AddListener(OnCreditsClicked);
        exitButton.onClick.AddListener(OnExitClicked);
    }

    private void OnPlayClicked()
    {
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