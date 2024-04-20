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
                transform.RotateAround(transform.position, transform.up, Input.GetAxis("Horizontal") / RotationDeg);
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
                movementVec.Normalize();
            }

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
