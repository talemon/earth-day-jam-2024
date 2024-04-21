using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawController : MonoBehaviour
{
    [HideInInspector] public Transform Target;
    [HideInInspector] public bool IsMoving = false;

    [SerializeField] private Transform InitialClawPos;
    [SerializeField] private float DescendingSpeed;
    [SerializeField] private Transform[] RopeSections;
    [SerializeField] private GameStateManager gameStateManager;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsMoving)
            return;

        transform.position -= transform.up * DescendingSpeed * Time.deltaTime;

        foreach (var section in RopeSections)
        {
            if (section.gameObject.activeSelf)
                continue;

            float dot = Vector3.Dot(section.position - transform.position, transform.up);
            if (dot > 0) // are we below the rope section?
                section.gameObject.SetActive(true);

        }

        float aimDistance = (transform.position - Target.position).magnitude;
        float aimDot = Vector3.Dot(Target.position - transform.position, transform.up);
        if (aimDistance < 0.1 || aimDot > 0) // are we below the aim?
            Reset();
    }

    void Reset()
    {
        IsMoving = false;
        transform.position = InitialClawPos.position;
        foreach (var section in RopeSections)
        {
            section.gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Garbage"))
        {
            var state = gameStateManager.GetGameState();
            if (state.TrashValues.ContainsKey(other.gameObject.tag))
            {
                state.Money += state.TrashValues[other.gameObject.tag];
            }
            if (state.TrashCollected.ContainsKey(other.gameObject.tag))
            {
                ++state.TrashCollected[other.gameObject.tag];
            }
            other.gameObject.SetActive(false);
        }
    }
}
