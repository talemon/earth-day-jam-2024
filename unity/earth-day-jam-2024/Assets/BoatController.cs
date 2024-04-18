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
    public float Drag = 0.1f;

    // Components
    protected Rigidbody rigidbody;
    protected UnityEngine.Quaternion startRotation;
    protected ParticleSystem particleSystem;
    protected Camera camera;

    // 
    protected UnityEngine.Vector3 cameraVelocity;

    public void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
        startRotation = motor.localRotation;
        camera = Camera.main;
    }


    public void FixedUpdate()
    {
        UnityEngine.Vector3 forceDirection = transform.forward;
        int steerDirection = 0;

        if (Input.GetKey(KeyCode.A))
            steerDirection = 1;
        if (Input.GetKey(KeyCode.D))
            steerDirection = -1;

        // Rotational force
        rigidbody.AddForceAtPosition(steerDirection * transform.right * SteerPower / 100f, motor.position);
        // rigidbody.AddTorque(steerDirection * transform.right * SteerPower);

        // get forward vector
        UnityEngine.Vector3 forward = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(1, 0, 1), transform.forward);

        // forward/backward acceleration
        if (Input.GetKey(KeyCode.W))
            rigidbody.AddForce(forward * (MaxSpeed * Power), ForceMode.Force);
            // ApplyForceToReachVelocity(rigidbody, forward * MaxSpeed, Power);
        if (Input.GetKey(KeyCode.S))
            rigidbody.AddForce(forward * (-MaxSpeed * Power), ForceMode.Force);
            // ApplyForceToReachVelocity(rigidbody, forward * -MaxSpeed, Power);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    // this function is from the YT video's GitHub repo and he doesn't explain any of it lol
    public static void ApplyForceToReachVelocity(Rigidbody rigidbody, UnityEngine.Vector3 velocity, float force = 1, ForceMode mode = ForceMode.Force)
    {
        if (force == 0 || velocity.magnitude == 0)
            return;

        velocity = velocity + velocity.normalized * 0.2f * rigidbody.drag;

        //force = 1 => need 1 s to reach velocity (if mass is 1) => force can be max 1 / Time.fixedDeltaTime
        force = Mathf.Clamp(force, -rigidbody.mass / Time.fixedDeltaTime, rigidbody.mass / Time.fixedDeltaTime);

        //dot product is a projection from rhs to lhs with a length of result / lhs.magnitude https://www.youtube.com/watch?v=h0NJK4mEIJU
        if (rigidbody.velocity.magnitude == 0)
        {
            rigidbody.AddForce(velocity * force, mode);
        }
        else
        {
            var velocityProjectedToTarget = (velocity.normalized * UnityEngine.Vector3.Dot(velocity, rigidbody.velocity) / velocity.magnitude);
            rigidbody.AddForce((velocity - velocityProjectedToTarget) * force, mode);
        }
    }

}
