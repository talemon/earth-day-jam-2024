using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public InteractableProp Prop;
    public GameObject Aim;
    public HookController Hook;

    public enum FishingRodState
    {
        Disabled,
        Aiming,
        Shooting
    };

    public FishingRodState State;

    private int _lastShootAxis;
    private Vector3 _hookDefaultPosition;

    // Start is called before the first frame update
    void Start()
    {
        _hookDefaultPosition = Hook.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (State != FishingRodState.Disabled && Prop.State != InteractableProp.InteractableState.Occupied)
        {
            State = FishingRodState.Disabled;
            Aim.SetActive(false);
            Hook.gameObject.SetActive(false);
            return;
        }

        if (State == FishingRodState.Disabled && Prop.State == InteractableProp.InteractableState.Occupied)
        {
            State = FishingRodState.Aiming;
            Aim.SetActive(true);
        }
        else if (State == FishingRodState.Aiming)
        {
            if (_lastShootAxis == 0 && Input.GetAxis("Shoot") > 0)
            {
                Aim.SetActive(false);
                Hook.gameObject.transform.position = _hookDefaultPosition;
                Hook.gameObject.SetActive(true);
                Hook.Target = Aim.transform.position;
                State = FishingRodState.Shooting;
            }
        }
        else if (State == FishingRodState.Shooting)
        {
            if (!Hook.InFlight)
            {
                State = FishingRodState.Aiming;
                Aim.SetActive(true);
            }
        }

        _lastShootAxis = (int)Input.GetAxis("Shoot");
    }
}
