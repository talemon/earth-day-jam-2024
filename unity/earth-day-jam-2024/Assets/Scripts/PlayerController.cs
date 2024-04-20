using UnityEngine;

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

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (State != PlayerState.Immovable)
        {
            // Kinematic allows us to stop the player moving while the boat is moving
            _rigidBody.isKinematic = false;
            transform.RotateAround(transform.position, transform.up, Input.GetAxis("Horizontal") / RotationDeg);

            Vector3 movementVec = transform.forward * Input.GetAxis("Vertical");
            switch (MovementMethod)
            {
                case MovementMethodEnum.Physics:
                    _rigidBody.angularVelocity = Vector3.zero;
                    if (movementVec.magnitude < 0.1f)
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
