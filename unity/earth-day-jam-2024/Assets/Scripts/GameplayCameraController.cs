using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public enum EGameplayCamera
{
    Player,
    Boat,
    Claw,
    Harpoon
}

public class GameplayCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startingCamera;
    [SerializeField] private CinemachineVirtualCamera playerCamera;
    [SerializeField] private CinemachineVirtualCamera boatCamera;
    [SerializeField] private CinemachineVirtualCamera clawCamera;
    [SerializeField] private CinemachineVirtualCamera harpoonCamera;
    
    public VolumeProfile profile;
    
    private DepthOfField _depthOfField;
    
    private void Start()
    {
        profile.TryGet(out _depthOfField);
        SwitchToCamera(EGameplayCamera.Player);
        
        playerCamera.MoveToTopOfPrioritySubqueue();
        startingCamera.enabled = false;
    }

    public void SwitchToCamera(EGameplayCamera cameraSelection)
    {
        _depthOfField.active = cameraSelection == EGameplayCamera.Player;
        
        switch (cameraSelection)
        {
            case EGameplayCamera.Player:
                playerCamera.MoveToTopOfPrioritySubqueue();
                break;
            case EGameplayCamera.Boat:
                boatCamera.MoveToTopOfPrioritySubqueue();
                break;
            case EGameplayCamera.Claw:
                clawCamera.MoveToTopOfPrioritySubqueue();
                break;
            case EGameplayCamera.Harpoon:
                harpoonCamera.MoveToTopOfPrioritySubqueue();
                break;
            default:
                Debug.LogError("Invalid camera selection");
                break;
        }
    }
}