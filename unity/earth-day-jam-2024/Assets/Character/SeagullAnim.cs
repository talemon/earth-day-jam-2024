using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeagullAnim : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Animator animator = GetComponent<Animator>();
        float offset = Random.Range(0f, 2f);
        
        animator.Play("seagull", 0, offset);
    }
}
