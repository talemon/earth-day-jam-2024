using UnityEngine;

public class ShipWheelInteractable : LockingInteractable
{
    [SerializeField] private KinematicBoat boatController;

    protected override void OnInteract(GameObject player)
    {
        if (State == InteractableObjectState.Available)
        {
            boatController.enabled = true;
            cameraController.SwitchToCamera(EGameplayCamera.Boat);

            var playerAnim = player.GetComponentInChildren<PlayerAnim>();
            if (playerAnim != null)
            {
                playerAnim.EnterSteering();
            }
        }
        else if(State == InteractableObjectState.Busy)
        {
            boatController.enabled = false;
            cameraController.SwitchToCamera(EGameplayCamera.Player);

            var playerAnim = player.GetComponentInChildren<PlayerAnim>();
            if (playerAnim != null)
            {
                playerAnim.ExitSteering();
            }
        }
        base.OnInteract(player);
    }
}