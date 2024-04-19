using UnityEngine;

public class BoatController : MonoBehaviour
{
    // visible and editable properties
    public Transform Motor;
    public Transform DeckCamPos;
    public Transform TopCamPos;
    public InteractableProp SteeringWheel;
    public float SteerPower = 50f;
    public float Power = 0.25f;
    public float MaxSpeed = 18f;
    
    private bool _motorOn = true;

    // Components
    private Rigidbody _rigidbody;
    private Camera _camera;
    private float _lerpSpeed = 0.01f;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _camera = Camera.main;
    }

    private void FixedUpdate()
    {
        Vector3 localVelocity = _rigidbody.velocity * Vector3.Dot(_rigidbody.velocity.normalized, transform.forward);

        int steerDirection = 0;

        if (_motorOn)
        {
            // Gotta make sure the turning inverts when going backwards like a real rudder
            if (Input.GetAxis("Horizontal") < 0)
            {
                if (Input.GetAxis("Fire1") > 0 || Input.GetAxis("Vertical") > 0)
                    steerDirection = 1;
                else if (Input.GetAxis("Fire2") > 0 || Input.GetAxis("Vertical") < 0)
                    steerDirection = -1;
            } 

            if (Input.GetAxis("Horizontal") > 0)
            {
                if (Input.GetAxis("Fire1") > 0 || Input.GetAxis("Vertical") > 0)
                    steerDirection = -1;
                else if (Input.GetAxis("Fire2") > 0 || Input.GetAxis("Vertical") < 0)
                    steerDirection = 1;
            }
        }

        // Make sure we can't turn the boat whithout moving
        if (localVelocity.magnitude < 0.5f)
            steerDirection = 0;

        // Rotational force
        _rigidbody.AddForceAtPosition(transform.right * (steerDirection * SteerPower) / 100f, Motor.position);

        // get forward vector
        Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

        if (_motorOn)
        {
            // // forward/backward acceleration with either analogue stick or triggers
            if (Input.GetAxis("Fire1") > 0 || Input.GetAxis("Vertical") > 0)
                _rigidbody.AddForce(forward * (MaxSpeed * Power), ForceMode.Force);
            if (Input.GetAxis("Fire2") > 0 || Input.GetAxis("Vertical") < 0)
                _rigidbody.AddForce(forward * (-MaxSpeed * Power), ForceMode.Force);
        }
    }

    private void Update()
    {
        if (SteeringWheel.State != InteractableProp.InteractableState.Occupied)
        {
            _motorOn = false;
            Vector3 smoothPosition = Vector3.Lerp(_camera.transform.position, DeckCamPos.position, _lerpSpeed);
            _camera.transform.position = smoothPosition;

            Quaternion smoothRotation = Quaternion.Lerp(_camera.transform.rotation, DeckCamPos.rotation, _lerpSpeed);
            _camera.transform.rotation = smoothRotation;
        }
        else
        {
            _motorOn = true;
            Vector3 smoothPosition = Vector3.Lerp(_camera.transform.position, TopCamPos.position, _lerpSpeed);
            _camera.transform.position = smoothPosition;

            Quaternion smoothRotation = Quaternion.Lerp(_camera.transform.rotation, TopCamPos.rotation, _lerpSpeed);
            _camera.transform.rotation = smoothRotation;
        }
    }
}
