using Gameplay;
using UnityEngine;

[CreateAssetMenu(fileName = "gameStateManager", menuName = "GameData/GameState Manager", order = 1)]
public class GameStateManager : ScriptableObject
{
    public TrashData[] trashes;
    public int startingMoney = 500;
    public float moneyDecayInterval = 10f;
    
    private GameState _gameState = new GameState();

    public GameState GetGameState() => _gameState;
    
    public void Initialize()
    {
        _gameState = new GameState
        {
            Money = startingMoney
        };
    }
}