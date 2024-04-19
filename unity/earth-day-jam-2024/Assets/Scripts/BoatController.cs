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
        rigidbody.AddForceAtPosition(steerDirection * transform.right * SteerPower / 100f, Motor.position);

        // get forward vector
        UnityEngine.Vector3 forward = UnityEngine.Vector3.Scale(new UnityEngine.Vector3(1, 0, 1), transform.forward);

        if (motorOn)
        {
            // // forward/backward acceleration with either analogue stick or triggers
            if (Input.GetAxis("Fire1") > 0 || Input.GetAxis("Vertical") > 0)
                rigidbody.AddForce(forward * (MaxSpeed * Power), ForceMode.Force);
            if (Input.GetAxis("Fire2") > 0 || Input.GetAxis("Vertical") < 0)
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
