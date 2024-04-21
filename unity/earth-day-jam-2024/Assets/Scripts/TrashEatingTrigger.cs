using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEatingTrigger : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "SmallTrash")
        {
            var state = gameStateManager.GetGameState();
            if (state.TrashValues.ContainsKey(other.gameObject.tag))
            {
                state.Money += state.TrashValues[other.gameObject.tag];
            }
            if (state.TrashCollected.ContainsKey(other.gameObject.tag))
            {
                ++state.TrashCollected[other.gameObject.tag];
            }
            Destroy(other.gameObject);
        }
    }
}
