using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;

public class BoatController : MonoBehaviour
{
    // visible and editable properties
    public Transform Motor;
    public Transform DeckCamPos;
    public float SteerPower = 500f;
    public float Power = 5f;
    public float MaxSpeed = 10f;

    bool lerpToDeck, lerpToSky = false;

    // Components
    protected Rigidbody rigidbody;
    // protected UnityEngine.Quaternion startRotation;
    protected ParticleSystem particleSystem;
    protected Camera camera;
    protected UnityEngine.Vector3 cameraStartPos;
    protected UnityEngine.Quaternion cameraStartRot;
    protected float lerpSpeed = 0.01f; 

    public void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        rigidbody = GetComponent<Rigidbody>();
        // startRotation = Motor.localRotation;
        camera = Camera.main;
        cameraStartPos = camera.transform.position;
        cameraStartRot = camera.transform.rotation;
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
        rigidbody.AddForceAtPosition(steerDirection * transform.right * SteerPower / 100f, Motor.position);

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
        if (Input.GetKeyDown(KeyCode.Space))   
        {
            CameraLerp();
        }

        if (lerpToDeck)
        {
            UnityEngine.Vector3 smoothPosition = UnityEngine.Vector3.Lerp(camera.transform.position, DeckCamPos.position, lerpSpeed);
            camera.transform.position = smoothPosition;

            UnityEngine.Quaternion smoothRotation = UnityEngine.Quaternion.Lerp(camera.transform.rotation, DeckCamPos.rotation, lerpSpeed);
            camera.transform.rotation = smoothRotation;
        }
        if (lerpToSky)
        {
            UnityEngine.Vector3 smoothPosition = UnityEngine.Vector3.Lerp(camera.transform.position, cameraStartPos, lerpSpeed);
            camera.transform.position = smoothPosition;

            UnityEngine.Quaternion smoothRotation = UnityEngine.Quaternion.Lerp(camera.transform.rotation, cameraStartRot, lerpSpeed);
            camera.transform.rotation = smoothRotation;
        }
    }

    void CameraLerp()
    {
        // probably need a better way of knowing which way to lerp lol
        if (!lerpToDeck)
        {
            lerpToDeck = true;
            lerpToSky = false;
        }
        else
        {
            lerpToDeck = false;
            lerpToSky = true;
        }
    }

}
