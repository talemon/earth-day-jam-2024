using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookAimConrols : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Transform staticHarpoonStand;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        Vector3 movementVec = staticHarpoonStand.forward * Input.GetAxis("Vertical") + staticHarpoonStand.right * Input.GetAxis("Horizontal");
        _rigidBody.velocity = movementVec.normalized * velocity;
    }
}
