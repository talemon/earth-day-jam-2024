using System;
using UnityEngine;

public class KinematicBoat : MonoBehaviour
{
    [SerializeField] private float maxSpeed = 1f;
    [SerializeField] private float dragTime = 1f;
    [SerializeField] private float acceleration = 1f;
    [SerializeField] private float rotationSpeed = 1f;
    
    private Vector3 _velocity;
    private Rigidbody _rigidbody;
    private Vector3 _dragVelocity;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        var lateralInput = Input.GetAxis("Horizontal") * Time.fixedDeltaTime;
        
        var rot = _rigidbody.rotation * Quaternion.AngleAxis(lateralInput * rotationSpeed, Vector3.up);
        _rigidbody.MoveRotation(rot);

        var forwardInput = Input.GetAxis("Vertical") * Time.fixedDeltaTime;
        var dir = transform.forward * (forwardInput * acceleration);

        _velocity = Vector3.SmoothDamp(_velocity, Vector3.zero, ref _dragVelocity, dragTime);
        
        _velocity += dir;
        _velocity = Vector3.ClampMagnitude(_velocity, maxSpeed);

        _rigidbody.MovePosition(_rigidbody.position + _velocity);
    }

    private void OnDisable()
    {
        _velocity = Vector3.zero;
    }
}