using MenuScripts;
using System.Collections.Generic;
using UnityEngine;

public class GameState
{
    public string ShipName { get; set; }
    public int Money { get; set; }
    public bool GameOverTrigger { get; set; }

    public Dictionary<string, int> TrashCollected = new()
    {
       { "SmallTrash", 0 },
       { "BigTrash", 0 },
    };

    public Dictionary<string, int> TrashValues = new()
    {
       { "SmallTrash", 10 },
       { "BigTrash", 100 },
    };
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