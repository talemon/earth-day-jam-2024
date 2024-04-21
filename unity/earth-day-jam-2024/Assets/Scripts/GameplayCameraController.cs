using Cinemachine;
using UnityEngine;

public enum EGameplayCamera
{
    Player,
    Boat
}

public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera boatCamera;

    private void Start()
    {
        SwitchToCamera(EGameplayCamera.Player);
    }

    public void SwitchToCamera(EGameplayCamera cameraSelection)
    {
        switch (cameraSelection)
        {
            case EGameplayCamera.Player:
                playerCamera.Priority = 10;
                boatCamera.Priority = 1;
                break;
            case EGameplayCamera.Boat:
                playerCamera.Priority = 1;
                boatCamera.Priority = 10;
                break;
            default:
                Debug.LogError("Invalid camera selection");
                break;
        }
    }
}