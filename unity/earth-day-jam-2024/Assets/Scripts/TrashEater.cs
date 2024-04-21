using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEater : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    //Detect collisions between the GameObjects with Colliders attached
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "SmallTrash")
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 objectPosition = Vector3.Normalize(collision.gameObject.transform.position - transform.position);
            // Debug.Log("Collided object dot value: " + Vector3.Dot(forward, objectPosition));

            // If the trash hit the boat from the front
            if (Vector3.Dot(forward, objectPosition) > 0.8f)
            {
                var state = gameStateManager.GetGameState();
                if (state.TrashValues.ContainsKey(collision.gameObject.tag))
                {
                    state.Money += state.TrashValues[collision.gameObject.tag];
                }
                if (state.TrashCollected.ContainsKey(collision.gameObject.tag))
                {
                    ++state.TrashCollected[collision.gameObject.tag];
                }
                // Debug.Log("Boat hit small trash from front!");
                Destroy(collision.gameObject);
            }
        }
    }
}
