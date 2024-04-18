using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject Followee;
    public Vector3 Distance;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Followee.transform.position;
        transform.Translate(transform.up * Distance.y, Space.World);
        transform.Translate(Followee.transform.forward * -1 * Distance.x, Space.World);
        transform.LookAt(Followee.transform.position);
    }
}
