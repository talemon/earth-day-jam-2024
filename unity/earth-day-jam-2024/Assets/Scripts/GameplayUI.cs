using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text shipName;

    // Start is called before the first frame update
    void Start()
    {
        shipName.text = gameStateManager.GetGameState().ShipName;
    }
}
