using System.Collections.Generic;
using Gameplay;

public class GameState
{
    public string ShipName { get; set; }
    public int Money { get; set; } = 500;
    public bool GameOverTrigger { get; set; }

    public Dictionary<TrashData, int> CollectedTrash { get; } = new();
    
    public void AddTrash(TrashData trash)
    {
        if (trash == null)
        {
            return;
        }
        
        if (!CollectedTrash.TryAdd(trash, 1))
        {
            CollectedTrash[trash] += 1;
        }

        Money += trash.score;
    }
}