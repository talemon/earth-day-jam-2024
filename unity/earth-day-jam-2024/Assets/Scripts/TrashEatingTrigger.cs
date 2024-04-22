using Gameplay;
using UnityEngine;

public class TrashEatingTrigger : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Trash"))
        {
            var trashComp = other.GetComponent<Trash>();
            if (trashComp != null && trashComp.data.size == TrashSize.Small)
            {
                gameStateManager.GetGameState().AddTrash(trashComp.data);
                trashComp.Disappear();
            }
        }
    }
}
