using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerControls _playerControls;

    private PlayerController playerController;

    private bool isSubmitPressed = false;

    bool isInputDisabled = false;
    private static PlayerInputController instance;




    private void Awake() {
       _playerControls = new PlayerControls();
       _playerControls.Enable();

       playerController = GetComponent<PlayerController>();
       instance = this;
    }

    public static PlayerInputController GetInstance() {
        return instance;
    }

    private void OnEnable() {
        _playerControls.PlayerActions.Interact.performed += InteractionPerformed;
        _playerControls.PlayerActions.Interact.canceled += InteractionCanceled;
        _playerControls.DialogueActions.Submit.performed += SubmitPerformed;
        _playerControls.DialogueActions.Submit.canceled += SubmitCancelled;
        _playerControls.UIActions.Pause.performed += PausePerformed;
    }

    private void OnDisable() {
        _playerControls.PlayerActions.Interact.performed -= InteractionPerformed;
        _playerControls.PlayerActions.Interact.canceled -= InteractionCanceled;
        _playerControls.DialogueActions.Submit.performed -= SubmitPerformed;
        _playerControls.DialogueActions.Submit.canceled -= SubmitCancelled;
        _playerControls.UIActions.Pause.performed -= PausePerformed;
    }

    private void InteractionPerformed(InputAction.CallbackContext obj) {
        playerController.interact();
    }

    private void InteractionCanceled(InputAction.CallbackContext obj) {
        playerController.interactionCancel();
    }

    private void SubmitPerformed(InputAction.CallbackContext obj) {
        isSubmitPressed = true;
    }

    private void SubmitCancelled(InputAction.CallbackContext obj) {
        isSubmitPressed = false;
    }


    public bool GetSubmitPressed() {
        return isSubmitPressed;
    }

    private void PausePerformed(InputAction.CallbackContext obj) {
        if (!isInputDisabled) {
            PauseMenuManager.GetInstance().Pause();
            isInputDisabled = true;
        }
        else {
            PauseMenuManager.GetInstance().UnPause();
            isInputDisabled = false;
        }
    }

    public void DisableInput() {
        _playerControls.PlayerActions.Disable();
        isInputDisabled = true;
    }

    public void EnableInput() {
        _playerControls.PlayerActions.Enable();
        isInputDisabled = false;
    }

    public void FullDisable() {
        _playerControls.PlayerActions.Disable();
        _playerControls.UIActions.Disable();
        _playerControls.DialogueActions.Disable();
    }

    public void FullEnable() {
        _playerControls.PlayerActions.Enable();
        _playerControls.UIActions.Enable();
        _playerControls.DialogueActions.Enable();
    }
}
