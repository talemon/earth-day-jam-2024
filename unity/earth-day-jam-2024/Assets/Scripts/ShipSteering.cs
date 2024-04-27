using UnityEngine;

public class ShipSteering : MonoBehaviour
{
    public GameObject Ship;
    public InteractableProp Prop;
    public float RotationDeg;

    void Update()
    {
        if (Prop.State == InteractableProp.InteractableState.Occupied)
        {
            Ship.transform.RotateAround(Ship.transform.position, Ship.transform.up, Input.GetAxis("Horizontal") * RotationDeg);
        }
    }
}
