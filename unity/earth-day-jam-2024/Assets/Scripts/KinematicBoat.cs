using System;
using UnityEngine;

public class KinematicBoat : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    // [SerializeField] private float dragTime = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    [SerializeField] private ShipWheelInteractable SteeringWheel;
    [SerializeField] private Transform motorPosition;
    
    private Vector3 _velocity;
    private Rigidbody _rigidbody;
    // private Vector3 _dragVelocity;
    private bool _motorOn;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // var lateralInput = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        
        // var rot = _rigidbody.rotation * Quaternion.AngleAxis(lateralInput * rotationSpeed, Vector3.up);
        // _rigidbody.MoveRotation(rot);

        // var forwardInput = Input.GetAxis("Vertical") * Time.fixedDeltaTime;
        // var dir = transform.forward * (forwardInput * acceleration);

        // _velocity = Vector3.SmoothDamp(_velocity, Vector3.zero, ref _dragVelocity, dragTime);
        
        // _velocity += dir;
        // _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);

        // _rigidbody.MovePosition(_rigidbody.position + _velocity);


        //---------------- Physics Controls Below
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

            // get forward vector
            Vector3 forward = Vector3.Scale(new Vector3(1, 0, 1), transform.forward);

            // // forward/backward acceleration with either analogue stick or triggers
            if (Input.GetAxis("Fire1") > 0 || Input.GetAxis("Vertical") > 0)
                _rigidbody.AddForce(forward * (maxSpeed * acceleration), ForceMode.Force);
            if (Input.GetAxis("Fire2") > 0 || Input.GetAxis("Vertical") < 0)
                _rigidbody.AddForce(forward * (-maxSpeed * acceleration), ForceMode.Force);
        }

        // Make sure we can't turn the boat whithout moving
        if (localVelocity.magnitude < 0.5f)
            steerDirection = 0;

        // Rotational force
        _rigidbody.AddForceAtPosition(transform.right * (steerDirection * rotationSpeed) / 100f, motorPosition.position);
    }

    private void Update()
    {
        if (SteeringWheel.State != InteractableObjectState.Busy)
        {
            _motorOn = false;
        }
        else
        {
            _motorOn = true;
        }
    }

    private void OnDisable()
    {
        _velocity = Vector3.zero;
    }
}