using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public Vector3 Target;
    public float FlightTime;
    public float CurveHeight;

    public bool InFlight = false;

    private Vector3 _initialPosition;
    private float _initialDistanceToTarget;
    private float _currentFlightTime;
    private Vector3 _lerpPosition;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        _initialPosition = transform.position;
        _lerpPosition = _initialPosition;
        _initialDistanceToTarget = (transform.position - Target).magnitude;
        _currentFlightTime = 0;
        InFlight = true;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceToTarget = (_lerpPosition - Target).magnitude;
        // hack tbh, either of them should suffice if everything works correctly
        if (distanceToTarget < 0.01 || _currentFlightTime >= FlightTime)
        {
            gameObject.SetActive(false);
            InFlight = false;
            transform.position = _initialPosition;
            return;
        }

        _lerpPosition = Vector3.Lerp(_initialPosition, Target, _currentFlightTime / FlightTime);

        // Overengineered sin curve 
        float additionalHeight = CurveHeight * Mathf.Sin((_initialDistanceToTarget - distanceToTarget) * Mathf.PI / _initialDistanceToTarget);
        transform.position = _lerpPosition + new Vector3(0, additionalHeight, 0);

        _currentFlightTime += Time.deltaTime;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
