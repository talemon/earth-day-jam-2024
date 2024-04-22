using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text moneyText;

    private float _moneyDecayInterval = 10f;
    private float _deductTimer;
    
    private void Start()
    {
        gameStateManager.GetGameState().GameOverTrigger = false;
        _moneyDecayInterval = gameStateManager.moneyDecayInterval;
        _deductTimer = _moneyDecayInterval;
    }

    private void Update()
    {
        var gameState = gameStateManager.GetGameState();
        moneyText.text = gameState.Money.ToString();

        if (gameState.GameOverTrigger)
            return;
        
        _deductTimer -= Time.deltaTime;
        if (_deductTimer <= 0f)
        {
            gameState.Money = Mathf.Clamp(gameState.Money - 100, 0, int.MaxValue);
            
            if (gameState.Money <= 0)
            {
                gameState.GameOverTrigger = true;
                return;
                // Debug.Log("Business out of money!");
            }
            
            _deductTimer = _moneyDecayInterval;
        }
    }
}
