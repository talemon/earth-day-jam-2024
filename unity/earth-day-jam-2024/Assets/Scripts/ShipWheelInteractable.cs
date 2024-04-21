using UnityEngine;

public class ShipWheelInteractable : InteractableObject
{
    [SerializeField] private Transform captainSeat;
    [SerializeField] private KinematicBoat boatController;
    [SerializeField] private GameplayCameraController cameraController;

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
            boatController.enabled = true;
            cameraController.SwitchToCamera(EGameplayCamera.Boat);
        }
        else if(State == InteractableObjectState.Busy)
        {
            State = InteractableObjectState.Available;
            
            ReleasePlayer(player);
            boatController.enabled = false;
            cameraController.SwitchToCamera(EGameplayCamera.Player);
        }
    }

    private void CapturePlayer(GameObject player)
    {
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Locked;

        var playerController = player.GetComponent<PlayerController>();
        playerController.enabled = false;

        _playerParent = player.transform.parent;
        player.transform.SetParent(captainSeat);
        player.transform.SetPositionAndRotation(captainSeat.position, captainSeat.rotation);

        var playerAnim = player.GetComponentInChildren<PlayerAnim>();
        if (playerAnim != null)
        {
            playerAnim.EnterSteering();
        }
    }

    private void ReleasePlayer(GameObject player)
    {
        player.transform.SetParent(_playerParent);
        
        var playerController = player.GetComponent<PlayerController>();
        playerController.enabled = true;
        
        var interactionComp = player.GetComponent<PlayerInteractionComponent>();
        interactionComp.State = PlayerInteractionState.Free;
        
        var playerAnim = player.GetComponentInChildren<PlayerAnim>();
        if (playerAnim != null)
        {
            playerAnim.ExitSteering();
        }
    }
}