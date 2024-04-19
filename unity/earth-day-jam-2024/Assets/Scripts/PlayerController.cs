using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.WSA;

public class PlayerController : MonoBehaviour
{
    public float TranslationSpeed;
    public float PhysicalSpeed;
    public float RotationDeg;

    public enum MovementMethodEnum
    {
        Physics,
        DirectPositionSet
    };

    public MovementMethodEnum MovementMethod;

    public enum PlayerState
    {
        Default,
        Immovable
    };

    public PlayerState State = PlayerState.Default;

    private Rigidbody _rigidBody;

    // Start is called before the first frame update
    void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State != PlayerState.Immovable)
        {
            // Kineamtic allows us to stop the player moving while the boat is moving
            _rigidBody.isKinematic = false;
            transform.RotateAround(transform.position, transform.up, Input.GetAxis("Horizontal") / RotationDeg);

            Vector3 movementVec = transform.forward * Input.GetAxis("Vertical");
            switch (MovementMethod)
            {
                case MovementMethodEnum.Physics:
                    _rigidBody.angularVelocity = Vector3.zero;
                    if (movementVec.magnitude < 0.1)
                    {
                        _rigidBody.velocity = Vector3.zero;
                    }
                    else
                    {
                        _rigidBody.velocity = movementVec * PhysicalSpeed;
                    }
                    break;
                case MovementMethodEnum.DirectPositionSet:
                    transform.Translate(movementVec * TranslationSpeed, Space.World);
                    break;

            }
        }
        else
        {
            _rigidBody.velocity = Vector3.zero;
        }
    }
}
