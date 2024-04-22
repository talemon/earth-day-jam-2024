using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text moneyText;

    private float _deductTimer;
    
    private void Start()
    {
        gameStateManager.GetGameState().IsGameOver = false;
        _deductTimer = gameStateManager.moneyDecayIntervalSeconds;
    }

    private void Update()
    {
        var gameState = gameStateManager.GetGameState();
        moneyText.text = gameState.Money.ToString();

        if (gameState.IsGameOver)
            return;
        
        _deductTimer -= Time.deltaTime;
        if (_deductTimer <= 0f)
        {
            gameState.Money = Mathf.Clamp(gameState.Money - gameStateManager.moneyDecayAmount, 0, int.MaxValue);
            
            if (gameState.Money <= 0)
            {
                gameState.IsGameOver = true;
                return;
                // Debug.Log("Business out of money!");
            }
            
            _deductTimer = gameStateManager.moneyDecayIntervalSeconds;
        }
    }
}
