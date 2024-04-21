using MenuScripts;
using UnityEngine;

public class LockingInteractable : InteractableObject
{
    [SerializeField] protected Transform playerLockPosition;
    [SerializeField] protected GameplayCameraController cameraController;

    private Transform _playerParent;
    
    protected override bool CanInteract()
    {
        return enabled;
    }

    protected override void OnInteract(GameObject player)
    {
        base.OnInteract(player);
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

    protected void CapturePlayer(GameObject player)
    {
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Locked;

        var playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false;

        _playerParent = player.transform.parent;
        player.transform.SetParent(playerLockPosition);
        player.transform.SetPositionAndRotation(playerLockPosition.position, playerLockPosition.rotation);
        
        HUDManager.Instance?.ShowToolExitPrompt(true);
    }

    protected void ReleasePlayer(GameObject player)
    {
        player.transform.SetParent(_playerParent);
        
        var playerController = player.GetComponent<PlayerController>();
        playerController.enabled = true;
        
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Free;
        
        HUDManager.Instance?.ShowToolExitPrompt(false);
    }
}