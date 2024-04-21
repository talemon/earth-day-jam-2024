using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEatingTrigger : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log($"Boat hit {other.gameObject.tag}");
        if (other.gameObject.tag == "SmallTrash")
        {
            gameStateManager.GetGameState().Money += 10;
            gameStateManager.GetGameState().SmallTrashCollected += 1;
            Destroy(other.gameObject);
        }
    }
}
