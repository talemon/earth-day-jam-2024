using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindTurbineRotation : MonoBehaviour
{
    [SerializeField] private float RotationSpeedDeg;
    private float _rotationSpeed;
    // Start is called before the first frame update
    void Start()
    {
        _rotationSpeed = RotationSpeedDeg * Random.Range(0.6f, 1.3f);
        transform.RotateAround(transform.position, transform.forward,
                 RotationSpeedDeg * Random.Range(0.1f, 0.3f));
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, transform.forward, _rotationSpeed * Time.deltaTime);
    }
}
