using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text moneyText;

    private void Start()
    {
        gameStateManager.GetGameState().Money = 0;
        gameStateManager.GetGameState().SmallTrashCollected = 0;
    }

    private void Update()
    {
        moneyText.text = gameStateManager.GetGameState().Money.ToString();
    }
}
