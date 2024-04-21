using Cinemachine;
using UnityEngine;

public enum EGameplayCamera
{
    Player,
    Boat,
    Claw,
    Harpoon
}

public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera boatCamera;
    [SerializeField] private CinemachineVirtualCamera clawCamera;
    [SerializeField] private CinemachineVirtualCamera harpoonCamera;

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
                clawCamera.Priority = 1;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Boat:
                playerCamera.Priority = 1;
                boatCamera.Priority = 10;
                clawCamera.Priority = 1;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Claw:
                playerCamera.Priority = 1;
                boatCamera.Priority = 1;
                clawCamera.Priority = 10;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Harpoon:
                playerCamera.Priority = 1;
                boatCamera.Priority = 1;
                clawCamera.Priority = 1;
                harpoonCamera.Priority = 10;
                break;
            default:
                Debug.LogError("Invalid camera selection");
                break;
        }
    }
}