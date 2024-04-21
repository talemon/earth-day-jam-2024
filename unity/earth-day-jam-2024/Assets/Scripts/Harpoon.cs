using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HarpoonState
{
    Disabled,
    Aiming,
    Shooting
};

public class Harpoon : MonoBehaviour
{
    public InteractableObject Prop;
    public GameObject Aim;
    public HookController Arrow;

    public HarpoonState State;

    private int _lastShootAxis;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (State != HarpoonState.Disabled && Prop.State != InteractableObjectState.Busy)
        {
            State = HarpoonState.Disabled;
            Aim.SetActive(false);
            return;
        }

        if (State == HarpoonState.Disabled && Prop.State == InteractableObjectState.Busy)
        {
            State = HarpoonState.Aiming;
            Aim.SetActive(true);
        }
        else if (State == HarpoonState.Aiming)
        {
            if (_lastShootAxis == 0 && Input.GetAxis("Shoot") > 0)
            {
                Arrow.Target = Aim.transform.position;
                Aim.SetActive(false);
                Arrow.StartFlying();
                State = HarpoonState.Shooting;
            }
        }
        else if (State == HarpoonState.Shooting)
        {
            if (!Arrow.InFlight)
            {
                State = HarpoonState.Aiming;
                Aim.SetActive(true);
            }
        }

        _lastShootAxis = (int)Input.GetAxis("Shoot");
    }
}