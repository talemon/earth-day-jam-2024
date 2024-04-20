using System.Linq;
using UnityEngine;

public enum PlayerInteractionState
{
    Free,
    Locked
}

public class PlayerInteractionComponent : MonoBehaviour
{
    [SerializeField] private float interactionDistance = 3f;
    [SerializeField] private LayerMask layerMask;
    
    private readonly Collider[] _colliderCache = new Collider[5];
    private InteractableObject _closestInteractable;
    
    public PlayerInteractionState State { get; set; }

    private InteractableObject ClosestInteractable
    {
        get => _closestInteractable;
        set
        {
            if (_closestInteractable == value)
                return;
            if (_closestInteractable != null)
            {
                _closestInteractable.IsCurrentTarget = false;
            }
            
            _closestInteractable = value;
            
            if (_closestInteractable != null)
            {
                _closestInteractable.IsCurrentTarget = true;
            }
        }
    }

    private void FixedUpdate()
    {
        if (State == PlayerInteractionState.Locked)
        {
            return;
        }
        
        var hitCount = Physics.OverlapSphereNonAlloc(transform.position, interactionDistance, _colliderCache, layerMask);

        if (hitCount > 0)
        {
            var closestInteractable = _colliderCache.Take(hitCount)
                .Select(col => col.GetComponentInParent<InteractableObject>())
                .Where(comp => comp != null)
                .OrderBy(interactableComp => Vector3.Distance(interactableComp.transform.position, this.transform.position)).FirstOrDefault();

            ClosestInteractable = closestInteractable;
        }
        else
        {
            ClosestInteractable = null;
        }
    }

    private void Update()
    {
        if (Input.GetButtonUp("Action") && _closestInteractable != null)
        {
            _closestInteractable.Interact(gameObject);
        }
    }
}