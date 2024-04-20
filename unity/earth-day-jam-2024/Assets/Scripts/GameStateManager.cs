using UnityEngine;

public class GameState
{
    public string ShipName { get; set; }
}

[CreateAssetMenu(fileName = "gameStateManager", menuName = "GameData/GameState Manager", order = 1)]
public class GameStateManager : ScriptableObject
{
    private GameState _gameState = new GameState();

    public GameState GetGameState() => _gameState;
    
    public void Initialize()
    {
        _gameState = new GameState();
    }
}