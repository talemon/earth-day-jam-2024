using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float TranslationSpeed;
    public float PhysicalSpeed;
    public float RotationDeg;

    public PlayerAnim PlayerAnimScript;

    public enum MovementMethodEnum
    {
        Physics,
        DirectPositionSet
    };

    public MovementMethodEnum MovementMethod = MovementMethodEnum.Physics;

    public enum ControlsEnum
    {
        ButtonFacing,
        Tank
    };
    public ControlsEnum ControlsType = ControlsEnum.ButtonFacing;

    public enum PlayerState
    {
        Default,
        Immovable
    };

    public PlayerState State = PlayerState.Default;

    private Rigidbody _rigidBody;
    private Vector3 currMovementVec_;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (State != PlayerState.Immovable)
        {
            Vector3 movementVec = Vector3.zero;
            if (ControlsType == ControlsEnum.Tank)
            {
                transform.RotateAround(transform.position, transform.up, Input.GetAxis("Horizontal") * RotationDeg  * Time.deltaTime);
                movementVec = transform.forward * Input.GetAxis("Vertical");
            }
            else if (ControlsType == ControlsEnum.ButtonFacing)
            {
                Vector3 forwardVec = Input.GetAxis("Vertical") * Vector3.Scale(Camera.main.transform.forward, new Vector3(1, 0, 1));
                Vector3 rightVec = Input.GetAxis("Horizontal") * Vector3.Scale(Camera.main.transform.right, new Vector3(1, 0, 1));
                movementVec = forwardVec + rightVec;

                if (movementVec != Vector3.zero)
                {
                    Quaternion rotation = Quaternion.LookRotation(movementVec, Vector3.up);
                    transform.rotation = rotation;
                }
            }

            if (movementVec.magnitude > 1)
            {
                // Happens when 2 buttons are pressed at the same time
                movementVec.Normalize();
            }

            PlayerAnimScript.SetSpeed(movementVec.magnitude);

            if (MovementMethod == MovementMethodEnum.DirectPositionSet)
            {
                transform.Translate(movementVec * TranslationSpeed * Time.deltaTime, Space.World);
            }
            currMovementVec_ = movementVec;
        }
    }

    private void FixedUpdate()
    {
        if (MovementMethod == MovementMethodEnum.Physics)
        {
            if (State != PlayerState.Immovable)
            {
                _rigidBody.angularVelocity = Vector3.zero;
                if (currMovementVec_.magnitude < 0.1f)
                {
                    _rigidBody.velocity = Vector3.zero;
                }
                else
                {
                    _rigidBody.velocity = currMovementVec_ * PhysicalSpeed;
                }
            }
            else
            {
                _rigidBody.velocity = Vector3.zero;
            }
        }
    }
}
