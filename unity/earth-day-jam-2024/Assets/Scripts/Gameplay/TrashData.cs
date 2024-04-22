using UnityEngine;

namespace Gameplay
{
    public enum TrashSize
    {
        Small,
        Big
    }
    
    [CreateAssetMenu(fileName = "trashData", menuName = "GameData/Trash Data", order = 1)]
    public class TrashData : ScriptableObject
    {
        public string displayName;
        public int score = 1;
        public TrashSize size;
    }
}