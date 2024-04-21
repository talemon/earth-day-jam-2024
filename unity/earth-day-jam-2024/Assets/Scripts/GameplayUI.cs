using TMPro;
using UnityEngine;

public class GameplayUI : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private TMP_Text shipName;

    private void Start()
    {
        shipName.text = gameStateManager.GetGameState().ShipName;
    }
}
