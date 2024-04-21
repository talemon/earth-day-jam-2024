using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        gameStateManager.GetGameState().Money = 500;
        gameStateManager.GetGameState().GameOverTrigger = false;
        InvokeRepeating("Deductmoney", 10f, 10f);
    }

    private void Update()
    {
        moneyText.text = gameStateManager.GetGameState().Money.ToString();
        
        if (gameStateManager.GetGameState().Money <= 0 && !gameStateManager.GetGameState().GameOverTrigger)
        {
            CancelInvoke();
            gameStateManager.GetGameState().GameOverTrigger = true;
            // Debug.Log("Business out of money!");
        }
    }

    private void Deductmoney()
    {
        // Debug.Log("Deducting money!");
        gameStateManager.GetGameState().Money -= 100;
    }
}
