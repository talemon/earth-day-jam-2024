using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text shipName;
    [SerializeField] private TMP_Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        gameStateManager.GetGameState().Money = 0;
        gameStateManager.GetGameState().SmallTrashCollected = 0;
        shipName.text = gameStateManager.GetGameState().ShipName;
    }

    private void Update()
    {
        moneyText.text = gameStateManager.GetGameState().Money.ToString();
    }
}
