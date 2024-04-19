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
    private float _lastAdditionalHeight = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnEnable()
    {
        _initialPosition = transform.position;
        _initialDistanceToTarget = (transform.position - Target).magnitude;
        _currentFlightTime = 0;
        InFlight = true;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position -= new Vector3(0, _lastAdditionalHeight, 0);

        float distanceToTarget = (transform.position - Target).magnitude;
        // hack tbh, either of them should suffice if everything works correctly
        if (distanceToTarget < 0.01 || _currentFlightTime >= FlightTime)
        {
            gameObject.SetActive(false);
            InFlight = false;
            return;
        }

        transform.position = Vector3.Lerp(_initialPosition, Target, _currentFlightTime / FlightTime);

        // Overengineered sin curve 
        float additionalHeight = CurveHeight * Mathf.Sin((_initialDistanceToTarget - distanceToTarget) * Mathf.PI / _initialDistanceToTarget);
        transform.position += new Vector3(0, additionalHeight, 0);

        _currentFlightTime += Time.deltaTime;
        _lastAdditionalHeight = additionalHeight;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            collision.gameObject.SetActive(false);
        }
    }
}
