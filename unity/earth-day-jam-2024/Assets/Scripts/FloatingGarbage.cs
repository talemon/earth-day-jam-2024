using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatingGarbage : MonoBehaviour
{
    public float Amplitude;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.localPosition = new Vector3(0, Mathf.Sin(Time.realtimeSinceStartup) * Amplitude, 0);
    }
}
