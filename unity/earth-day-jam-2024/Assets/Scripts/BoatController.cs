using System.Collections;
using System.Collections.Generic;
using System.Numerics;
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

    bool lerpToDeck, lerpToSky = false;
    bool motorOn = true;

    // Components
    protected Rigidbody rigidbody;
    protected ParticleSystem particleSystem;
    protected Camera camera;
    protected float lerpSpeed = 0.01f; 
    protected bool actionAxisPressed = false;

    public void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
        camera = Camera.main;
    }


    public void FixedUpdate()
    {
        UnityEngine.Vector3 localVelocity = rigidbody.velocity * UnityEngine.Vector3.Dot(rigidbody.velocity.normalized, transform.forward);

        int steerDirection = 0;

        if (motorOn)
        {
            if (Input.GetKey(KeyCode.A))
                steerDirection = 1;
            if (Input.GetKey(KeyCode.D))
                steerDirection = -1;
        }

        // Make sure we can't turn the boat whithout moving
        if (localVelocity.magnitude < 0.5f)
            steerDirection = 0;

        // Rotational force
        rigidbody.AddForceAtPosition(steerDirection * transform.right * SteerPower / 100f, Motor.position);

        // get forward vector
        UnityEngine.Vector3 forward = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(1, 0, 1), transform.forward);

        if (motorOn)
        {
            // forward/backward acceleration
            if (Input.GetKey(KeyCode.W))
                rigidbody.AddForce(forward * (MaxSpeed * Power), ForceMode.Force);
            if (Input.GetKey(KeyCode.S))
                rigidbody.AddForce(forward * (-MaxSpeed * Power), ForceMode.Force);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (SteeringWheel.State != InteractableProp.InteractableState.Occupied)
        {
            motorOn = false;
            UnityEngine.Vector3 smoothPosition = UnityEngine.Vector3.Lerp(camera.transform.position, DeckCamPos.position, lerpSpeed);
            camera.transform.position = smoothPosition;

            UnityEngine.Quaternion smoothRotation = UnityEngine.Quaternion.Lerp(camera.transform.rotation, DeckCamPos.rotation, lerpSpeed);
            camera.transform.rotation = smoothRotation;
        }
        else
        {
            motorOn = true;
            UnityEngine.Vector3 smoothPosition = UnityEngine.Vector3.Lerp(camera.transform.position, TopCamPos.position, lerpSpeed);
            camera.transform.position = smoothPosition;

            UnityEngine.Quaternion smoothRotation = UnityEngine.Quaternion.Lerp(camera.transform.rotation, TopCamPos.rotation, lerpSpeed);
            camera.transform.rotation = smoothRotation;
        }
    }
}
