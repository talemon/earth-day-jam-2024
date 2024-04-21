using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimingGun : MonoBehaviour
{
    [SerializeField] private GameObject Target;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Target.activeInHierarchy)
            transform.LookAt(Target.transform);
    }
}
