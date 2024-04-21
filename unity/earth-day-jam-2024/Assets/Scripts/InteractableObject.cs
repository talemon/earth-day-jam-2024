using UnityEngine;

public enum InteractableObjectState
{
    Available,
    Busy,
}

public class InteractableObject : MonoBehaviour
{
    [SerializeField] private InteractionPrompt prompt;
    
    private InteractableObjectState _state;
    private bool _isCurrentTarget;

    public bool IsCurrentTarget
    {
        get => _isCurrentTarget;
        set
        {
            _isCurrentTarget = value;
            UpdatePrompt();
        }
    }

    public InteractableObjectState State
    {
        get => _state;
        protected set
        {
            _state = value;
            UpdatePrompt();
        }
    }

    protected void UpdatePrompt(bool immediate = false)
    {
        if (State == InteractableObjectState.Available && IsCurrentTarget)
        {
            prompt.Show(immediate);
        }
        else
        {
            prompt.Hide(immediate);
        }
    }

    private void Start()
    {
        if (prompt == null)
        {
            Debug.LogError("Interactable object is missing the prompt, disabling.");
            enabled = false;
            return;
        }

        UpdatePrompt(true);
    }

    public void Interact(GameObject player)
    {
        OnInteract(player);
    }

    protected virtual bool CanInteract()
    {
        return enabled && State == InteractableObjectState.Available;
    }

    protected virtual void OnInteract(GameObject player)
    {
        // Debug.Log($"Interacted with {gameObject.name}");
    }
}