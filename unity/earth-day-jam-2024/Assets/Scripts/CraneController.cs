using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CraneState
{
    Disabled,
    Aiming,
    Clawing
};

public class CraneController : MonoBehaviour
{
    [SerializeField] private CraneState State;

    [SerializeField] private float RotationSpeedDeg;
    [SerializeField] private float MaxRotationDot;
    [SerializeField] private Transform StaticParent;

    [SerializeField] private float ExtensionSpeed;
    [SerializeField] private float MaxExtension;
    [SerializeField] private Transform MovingBoon;
    [SerializeField] private Transform StaticBoon;
    
    [SerializeField] private InteractableObject Interactable;
    [SerializeField] private GameObject Aim;
    
    [SerializeField] private ClawController Claw;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (State != CraneState.Disabled && Interactable.State != InteractableObjectState.Busy)
        {
            State = CraneState.Disabled;
            Aim.SetActive(false);
            return;
        }

        if (State == CraneState.Disabled && Interactable.State == InteractableObjectState.Busy)
        {
            State = CraneState.Aiming;
            Aim.SetActive(true);
        }
        else if (State == CraneState.Aiming)
        {
            if (Input.GetButtonDown("Shoot"))
            {
                Claw.Target = Aim.transform;
                Claw.IsMoving = true;
                Aim.SetActive(false);
                State = CraneState.Clawing;
            }

            RotateCrane();
            ExtendBoon();
        }
        else if (State == CraneState.Clawing)
        {
            if (!Claw.IsMoving)
            {
                State = CraneState.Aiming;
                Aim.SetActive(true);
            }
        }
    }

    void ExtendBoon()
    {
        float axis = Input.GetAxis("Vertical");
        float distance = (StaticBoon.position - MovingBoon.position).magnitude;

        if (!((distance < 0.01 && axis < 0) || (distance >= MaxExtension && axis > 0)))
        {
            MovingBoon.position += MovingBoon.forward * (-axis * ExtensionSpeed * Time.deltaTime);
        }
    }

    void RotateCrane()
    {
        float dot = Vector3.Dot(StaticParent.right, transform.forward);
        float axis = Input.GetAxis("Horizontal");

        if (Mathf.Sign(dot) != Mathf.Sign(axis) || Mathf.Abs(dot) < MaxRotationDot)
        {
            transform.RotateAround(transform.position, transform.up,
                axis * RotationSpeedDeg * Time.deltaTime);
        }
    }
}
