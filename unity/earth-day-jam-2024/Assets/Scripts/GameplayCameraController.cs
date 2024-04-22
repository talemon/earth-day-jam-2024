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
    }

    public void SwitchToCamera(EGameplayCamera cameraSelection)
    {
        switch (cameraSelection)
        {
            case EGameplayCamera.Player:
                // _depthOfField.mode = new DepthOfFieldModeParameter(DepthOfFieldMode.Bokeh);
                _depthOfField.active = true;
                playerCamera.Priority = 10;
                boatCamera.Priority = 1;
                clawCamera.Priority = 1;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Boat:
                // _depthOfField.mode = new DepthOfFieldModeParameter(DepthOfFieldMode.Off);
                _depthOfField.active = false;
                playerCamera.Priority = 1;
                boatCamera.Priority = 10;
                clawCamera.Priority = 1;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Claw:
                // _depthOfField.mode = new DepthOfFieldModeParameter(DepthOfFieldMode.Off);
                _depthOfField.active = false;
                playerCamera.Priority = 1;
                boatCamera.Priority = 1;
                clawCamera.Priority = 10;
                harpoonCamera.Priority = 1;
                break;
            case EGameplayCamera.Harpoon:
                // _depthOfField.mode = new DepthOfFieldModeParameter(DepthOfFieldMode.Off);
                _depthOfField.active = false;
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