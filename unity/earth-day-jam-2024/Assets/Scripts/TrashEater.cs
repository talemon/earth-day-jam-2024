using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrashEater : MonoBehaviour
{
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
                // Debug.Log("Boat hit small trash from front!");
                Destroy(collision.gameObject);
            }
        }
    }
}
