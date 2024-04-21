﻿using UnityEngine;

public class ShootingInteractable : LockingInteractable
{
    [SerializeField] private EGameplayCamera ShootingCamera;
    private Vector3 _lastPlayerPosition;

    protected override void OnInteract(GameObject player)
    {
        if (State == InteractableObjectState.Available)
        {
            _lastPlayerPosition = player.transform.position;
            cameraController.SwitchToCamera(ShootingCamera);

            var playerAnim = player.GetComponentInChildren<PlayerAnim>();
            if (playerAnim != null)
            {
                playerAnim.EnterAiming();
            }
        }
        else if(State == InteractableObjectState.Busy)
        {
            cameraController.SwitchToCamera(EGameplayCamera.Player);

            var playerAnim = player.GetComponentInChildren<PlayerAnim>();
            if (playerAnim != null)
            {
                playerAnim.ExitAiming();
            }
            player.transform.position = _lastPlayerPosition;
        }
        base.OnInteract(player);
    }
}