using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAimConrols : MonoBehaviour
{
    public float Velocity;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementVec = transform.forward * Input.GetAxis("Vertical");
        _rigidBody.velocity = new Vector3(Input.GetAxis("Horizontal") * Velocity, 0, Input.GetAxis("Vertical") * Velocity);
    }
}
