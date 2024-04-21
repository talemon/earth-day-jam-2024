using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MoneyUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text moneyText;

    // Start is called before the first frame update
    void Start()
    {
        gameStateManager.GetGameState().Money = 0;
        gameStateManager.GetGameState().SmallTrashCollected = 0;
    }

    // Update is called once per frame
    void Update()
    {
        moneyText.text = gameStateManager.GetGameState().Money.ToString();
    }
}
