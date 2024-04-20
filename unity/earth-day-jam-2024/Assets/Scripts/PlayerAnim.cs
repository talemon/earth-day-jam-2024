using System;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    public Animator steeringWheelAnimator;
    
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int TurnDirection = Animator.StringToHash("TurnDirection");
    private static readonly int EnterSteeringAnim = Animator.StringToHash("EnterSteering");
    private static readonly int ExitSteeringAnim= Animator.StringToHash("ExitSteering");

    private void Update()
    {
        animator.SetFloat(TurnDirection, Input.GetAxis("Horizontal"));
        if (steeringWheelAnimator != null)
        {
            steeringWheelAnimator.SetFloat(TurnDirection, Input.GetAxis("Horizontal"));
        }
    }

    public void SetSpeed(float speed)
    {
        animator.SetFloat(Speed, speed);
    }

    public void EnterSteering()
    {
        animator.SetTrigger(EnterSteeringAnim);

        if (steeringWheelAnimator != null)
        {
            steeringWheelAnimator.SetTrigger(EnterSteeringAnim);
        }
    }

    public void ExitSteering()
    {
        animator.SetTrigger(ExitSteeringAnim);

        if (steeringWheelAnimator != null)
        {
            steeringWheelAnimator.SetTrigger(ExitSteeringAnim);
        }
    }
}