using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractableProp : MonoBehaviour
{
    public Image InteractionTooltip;
    public Text InteractionTooltipText;
    public GameObject PlayerPlaceHolder;
    public GameObject Model;
    
    private GameObject _player;

    public enum InteractableState
    {
        Idle,
        PlayerNearby,
        Occupied
    };

    public InteractableState State = InteractableState.Idle;

    CapsuleCollider _collider;
    // Start is called before the first frame update
    void Start()
    {
        _collider = Model.GetComponent<CapsuleCollider>();
        InteractionTooltip.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (State)
        {
            case InteractableState.PlayerNearby:
                if (Input.GetAxis("Action") > 0)
                {
                    InteractionTooltip.gameObject.SetActive(false);
                    _player.transform.SetPositionAndRotation(PlayerPlaceHolder.transform.position, PlayerPlaceHolder.transform.rotation);
                    _player.GetComponent<PlayerController>().State = PlayerController.PlayerState.Immovable;
                    Debug.Log("Activated!");
                    State = InteractableState.Occupied;
                }
                break;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (State == InteractableState.Occupied)
            return;

        if (other.gameObject.GetComponent<PlayerController>() == null)
            return;

        State = InteractableState.PlayerNearby;
        _player = other.gameObject;

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
        InteractionTooltipText.fontSize = (int)(buttonSizeScreen / 2f);
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
}
