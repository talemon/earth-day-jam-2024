using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipSteering : MonoBehaviour
{
    public GameObject Ship;
    public InteractableProp Prop;
    public float RotationDeg;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Prop.State == InteractableProp.InteractableState.Occupied)
        {
            Ship.transform.RotateAround(Ship.transform.position, Ship.transform.up, Input.GetAxis("Horizontal") * RotationDeg);
        }
    }
}
