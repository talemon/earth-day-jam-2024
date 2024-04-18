using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public PlayerController playerController;
    private Animator _animator;

    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (playerController.State != PlayerController.PlayerState.Immovable)
            _animator.SetFloat(Speed, Input.GetAxis("Vertical"));
    }
}
