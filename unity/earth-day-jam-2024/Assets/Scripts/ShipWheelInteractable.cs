using UnityEngine;

public class ShipWheelInteractable : InteractableObject
{
    [SerializeField] private Transform captainSeat;

    private Transform _playerParent;
    
    protected override bool CanInteract()
    {
        return enabled;
    }

    protected override void OnInteract(GameObject player)
    {
        if (State == InteractableObjectState.Available)
        {
            State = InteractableObjectState.Busy;
            
            CapturePlayer(player);
        }
        else if(State == InteractableObjectState.Busy)
        {
            State = InteractableObjectState.Available;
            
            ReleasePlayer(player);
        }
    }

    private void CapturePlayer(GameObject player)
    {
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Locked;

        var playerController = player.GetComponent<PlayerController>();
        playerController.State = PlayerController.PlayerState.Immovable;

        _playerParent = player.transform.parent;
        player.transform.SetParent(captainSeat);
        player.transform.SetPositionAndRotation(captainSeat.position, captainSeat.rotation);
    }

    private void ReleasePlayer(GameObject player)
    {
        player.transform.SetParent(_playerParent);
        
        var playerController = player.GetComponent<PlayerController>();
        playerController.State = PlayerController.PlayerState.Default;
        
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Free;
    }
}