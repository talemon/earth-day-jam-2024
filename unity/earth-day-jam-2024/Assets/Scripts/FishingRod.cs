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
    public GameObject HookPlaceHolder;

    private int _lastShootAxis;

    private void Update()
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
                Hook.gameObject.transform.position = HookPlaceHolder.transform.position;
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
