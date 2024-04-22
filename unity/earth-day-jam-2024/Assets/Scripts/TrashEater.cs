using System.Linq;
using Gameplay;
using UnityEngine;

public class TrashEater : MonoBehaviour
{
    [SerializeField] private GameStateManager gameStateManager;

    //Detect collisions between the GameObjects with Colliders attached
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("SmallTrash"))
        {
            Vector3 forward = transform.TransformDirection(Vector3.forward);
            Vector3 objectPosition = Vector3.Normalize(collision.gameObject.transform.position - transform.position);
            // Debug.Log("Collided object dot value: " + Vector3.Dot(forward, objectPosition));

            // If the trash hit the boat from the front
            if (Vector3.Dot(forward, objectPosition) > 0.8f)
            {
                var state = gameStateManager.GetGameState();
                // hacky way to not have to port this script completely - can
                state.AddTrash(gameStateManager.trashes.FirstOrDefault(data => data.size == TrashSize.Small));
                
                // Debug.Log("Boat hit small trash from front!");
                Destroy(collision.gameObject);
            }
        }
    }
}
