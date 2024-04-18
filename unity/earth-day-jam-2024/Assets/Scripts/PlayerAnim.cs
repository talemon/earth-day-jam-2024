using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    private Vector3 _lastPos;
    private Animator _animator;

    private Transform _player;

    private static readonly int Speed = Animator.StringToHash("Speed");

    // Start is called before the first frame update
    void Start()
    {
        _player = transform.parent;
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        float speed = Vector3.Distance(_player.localPosition, _lastPos);
        _animator.SetFloat(Speed, speed);
        print(speed);
        
        _lastPos = _player.localPosition;
    }
}
