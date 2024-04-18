using System;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator animator;
    private static readonly int Speed = Animator.StringToHash("Speed");
    private static readonly int EnterSteeringAnim = Animator.StringToHash("EnterSteering");
    private static readonly int ExitSteeringAnim= Animator.StringToHash("ExitSteering");

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        animator.SetFloat(Speed, Math.Abs(Input.GetAxis("Vertical")));
    }

    public void EnterSteering()
    {
        animator.SetTrigger(EnterSteeringAnim);
    }

    public void ExitSteering()
    {
        animator.SetTrigger(ExitSteeringAnim);
    }
}