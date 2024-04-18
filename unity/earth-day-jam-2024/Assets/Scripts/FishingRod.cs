using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishingRod : MonoBehaviour
{
    public InteractableProp Prop;
    public GameObject Aim;

    private bool _active = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!_active && Prop.State == InteractableProp.InteractableState.Occupied)
        {
            _active = true;
            Aim.SetActive(true);
        }
        else if (_active && Prop.State != InteractableProp.InteractableState.Occupied)
        {
            _active = false;
            Aim.SetActive(false);
        }
    }
}
