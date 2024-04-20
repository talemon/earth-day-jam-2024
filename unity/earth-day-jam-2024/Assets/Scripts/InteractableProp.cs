using System;
using UnityEngine;
using UnityEngine.UI;

public class InteractableProp : MonoBehaviour
{
    public Image InteractionTooltip;
    public Text InteractionTooltipText;

    public Image ExitPrompt;

    public GameObject PlayerPlaceHolder;
    public GameObject Model;
    
    private GameObject _player;
    private PlayerAnim _playerAnim;
    private Transform _playerParent;
    private int _lastActionAxis;


    public enum InteractableState
    {
        Idle,
        PlayerNearby,
        Occupied
    };
    
    public enum PropType
    {
        SteeringWheel,
        Fishing
    };

    public InteractableState State = InteractableState.Idle;
    public PropType type;

    CapsuleCollider _collider;

    private void Start()
    {
        _collider = Model.GetComponent<CapsuleCollider>();
        InteractionTooltip.gameObject.SetActive(false);
    }

    private void Update()
    {
        switch (State)
        {
            case InteractableState.PlayerNearby:
                if (_lastActionAxis == 0 && Input.GetAxis("Action") > 0)
                {
                    InteractionTooltip.gameObject.SetActive(false);
                    ExitPrompt.gameObject.SetActive(true);
                    _player.transform.SetPositionAndRotation(PlayerPlaceHolder.transform.position, PlayerPlaceHolder.transform.rotation);
                    _playerParent = _player.transform.parent;
                    _player.transform.SetParent(transform);
                    _player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Immovable;
                    Debug.Log("Activated!");
                    EnterEvent();
                    State = InteractableState.Occupied;
                }
                break;
            case InteractableState.Occupied:
                _player.transform.SetPositionAndRotation(PlayerPlaceHolder.transform.position, PlayerPlaceHolder.transform.rotation);
                if (_lastActionAxis == 0 && Input.GetAxis("Action") > 0)
                {
                    ExitPrompt.gameObject.SetActive(false);
                    _player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Default;
                    _player.transform.SetParent(_playerParent);
                    Debug.Log("Exited!");
                    ExitEvent();
                    State = InteractableState.Idle;
                }
                break;
        }
        _lastActionAxis = (int)Input.GetAxis("Action");
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (State == InteractableState.Occupied)
            return;

        if (other.gameObject.GetComponent<PlayerController>() == null)
            return;

        State = InteractableState.PlayerNearby;
        _player = other.gameObject;
        _playerAnim = _player.GetComponentInChildren<PlayerAnim>();

        InteractionTooltip.gameObject.SetActive(true);
        InteractionTooltip.transform.position = Camera.main.WorldToScreenPoint(transform.position);

        Vector3 buttonSizeVec = _collider.radius * 2 * Vector3.Scale(transform.right, transform.lossyScale);
        float buttonSizeWorld = buttonSizeVec.magnitude;
        Debug.Log($"button size world {buttonSizeWorld}");

        // The idea here is to project a line that's parallel to screen plane to get scale
        Vector3 buttonSizeScreenVec1 = Camera.main.WorldToScreenPoint(transform.position);
        Vector3 buttonSizeScreenVec2 = Camera.main.WorldToScreenPoint(transform.position + Camera.main.transform.right * buttonSizeWorld);

        const float uiScaleCoeff = 1.2f; // how big the button is compared to collider size on screen
        float buttonSizeScreen = uiScaleCoeff * (buttonSizeScreenVec1 - buttonSizeScreenVec2).magnitude;

        Debug.Log($"button size screen {buttonSizeScreen}");
        InteractionTooltip.rectTransform.sizeDelta = new Vector2(buttonSizeScreen, buttonSizeScreen);

        int fontSize = (int)(buttonSizeScreen / 2f);

        const int maxFontSize = 27; // if it's bigger, nothing shows
        if (fontSize < maxFontSize)
        {
            InteractionTooltipText.fontSize = fontSize;
        }
        else
        {
            InteractionTooltipText.fontSize = maxFontSize;
            float scale = (float)fontSize / (float)maxFontSize;
            InteractionTooltipText.gameObject.transform.localScale = new Vector3(scale, scale, 1);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (State == InteractableState.Occupied)
            return;

        InteractionTooltip.transform.position = Camera.main.WorldToScreenPoint(transform.position);
    }

    private void OnTriggerExit(Collider other)
    {
        if (State == InteractableState.Occupied)
            return;

        State = InteractableState.Idle;
        _player = null;
        InteractionTooltip.gameObject.SetActive(false);
    }

    private void EnterEvent()
    {
        switch (type)
        {
            case PropType.SteeringWheel:
                _playerAnim.EnterSteering();
                break;
            case PropType.Fishing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
    
    private void ExitEvent()
    {
        switch (type)
        {
            case PropType.SteeringWheel:
                _playerAnim.ExitSteering();
                break;
            case PropType.Fishing:
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }
}
