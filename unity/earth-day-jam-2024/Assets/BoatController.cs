using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    // visible and editable properties
    public Transform motor;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;

    // Components
    protected Rigidbody rigidbody;
    protected UnityEngine.Quaternion startRotation;
    protected ParticleSystem particleSystem;
    protected Camera camera;

    public void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
        startRotation = motor.localRotation;
        camera = Camera.main;
    }


    public void FixedUpdate()
    {
        UnityEngine.Vector3 localVelocity = rigidbody.velocity * UnityEngine.Vector3.Dot(rigidbody.velocity.normalized, transform.forward);

        int steerDirection = 0;

        if (Input.GetKey(KeyCode.A))
            steerDirection = 1;
        if (Input.GetKey(KeyCode.D))
            steerDirection = -1;

        // Make sure we can't turn the boat whithout moving
        if (localVelocity.magnitude < 0.5f)
            steerDirection = 0;

        // Rotational force
        rigidbody.AddForceAtPosition(steerDirection * transform.right * SteerPower / 100f, motor.position);

        // get forward vector
        UnityEngine.Vector3 forward = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(1, 0, 1), transform.forward);

        // forward/backward acceleration
        if (Input.GetKey(KeyCode.W))
            rigidbody.AddForce(forward * (MaxSpeed * Power), ForceMode.Force);
        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(forward * (-MaxSpeed * Power), ForceMode.Force);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
