using UnityEngine;

public class HookAimControls : MonoBehaviour
{
    [SerializeField] private float velocity;
    [SerializeField] private Transform staticHarpoonStand;

    private Rigidbody _rigidBody;

    private void Start()
    {
        _rigidBody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        Vector3 movementVec = staticHarpoonStand.forward * Input.GetAxis("Vertical") + staticHarpoonStand.right * Input.GetAxis("Horizontal");
        _rigidBody.velocity = movementVec.normalized * velocity;
    }
}
