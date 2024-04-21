using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookController : MonoBehaviour
{
    public Vector3 Target;
    public float Speed;
    public float CurveHeight;

    public bool InFlight = false;

    [SerializeField] private GameStateManager gameStateManager;
    [SerializeField] private Transform arrowHolder;
    private float _initialDistanceToTarget;
    private float _currentFlightTime;
    private float _targetFlightTime;
    private Vector3 _lerpPosition;
    private Transform _arrowParent;

    // Start is called before the first frame update
    public void StartFlying()
    {
        transform.SetPositionAndRotation(arrowHolder.position, arrowHolder.rotation);
        _lerpPosition = arrowHolder.position;
        _initialDistanceToTarget = (transform.position - Target).magnitude;
        _targetFlightTime = _initialDistanceToTarget / Speed;
        _currentFlightTime = 0;
        _arrowParent = transform.parent;
        transform.parent = null;
        InFlight = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!InFlight)
            return;

        float distanceToTarget = (_lerpPosition - Target).magnitude;
        // hack tbh, either of them should suffice if everything works correctly
        if (distanceToTarget < 0.01 || _currentFlightTime >= _targetFlightTime)
        {
            Debug.Log("Flight Done");
            InFlight = false;
            transform.parent = _arrowParent ;
            transform.SetPositionAndRotation(arrowHolder.position, arrowHolder.rotation);
            return;
        }

        _lerpPosition = Vector3.Lerp(arrowHolder.position, Target, _currentFlightTime / _targetFlightTime);

        // Overengineered sin curve (disabled)
        // float additionalHeight = CurveHeight * Mathf.Sin((_initialDistanceToTarget - distanceToTarget) * Mathf.PI / _initialDistanceToTarget);
        transform.position = _lerpPosition; //  + new Vector3(0, additionalHeight, 0);

        _currentFlightTime += Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            var state = gameStateManager.GetGameState();
            if (state.TrashValues.ContainsKey(other.gameObject.tag))
            {
                state.Money += state.TrashValues[other.gameObject.tag];
            }
            if (state.TrashCollected.ContainsKey(other.gameObject.tag))
            {
                ++state.TrashCollected[other.gameObject.tag];
            }
            other.gameObject.SetActive(false);
            _currentFlightTime = _targetFlightTime;
        }
    }
}
