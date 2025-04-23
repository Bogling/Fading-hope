using UnityEngine;
using UnityEngine.InputSystem;

public class DreamPlayerInputController : MonoBehaviour
{
    private PlayerControls _playerControls;

    private DreamPlayerController playerController;

    private bool isSubmitPressed = false;

    private static DreamPlayerInputController instance;
    bool isInputDisabled = false;

    private Vector2 horizontalInput;



    private void Awake() {
       _playerControls = new PlayerControls();
       _playerControls.Enable();

       playerController = GetComponent<DreamPlayerController>();
       instance = this;
    }

    public static DreamPlayerInputController GetInstance() {
        return instance;
    }

    private void OnEnable() {
        _playerControls.PlayerActions.Interact.performed += InteractionPerformed;
        _playerControls.PlayerActions.Interact.canceled += InteractionCanceled;
        _playerControls.DialogueActions.Submit.performed += SubmitPerformed;
        _playerControls.DialogueActions.Submit.canceled += SubmitCancelled;
        _playerControls.PlayerActions.HorizontalMovement.performed += HorizontalMovementPerformed;
        _playerControls.PlayerActions.Jump.performed += JumpPerformed;
        _playerControls.PlayerActions.AltAction.performed += AltActionPerformed;
        _playerControls.UIActions.Pause.performed += PausePerformed;
    }

    private void OnDisable() {
        _playerControls.PlayerActions.Interact.performed -= InteractionPerformed;
        _playerControls.PlayerActions.Interact.canceled -= InteractionCanceled;
        _playerControls.DialogueActions.Submit.performed -= SubmitPerformed;
        _playerControls.DialogueActions.Submit.canceled -= SubmitCancelled;
        _playerControls.PlayerActions.HorizontalMovement.performed -= HorizontalMovementPerformed;
        _playerControls.PlayerActions.Jump.performed -= JumpPerformed;
        _playerControls.PlayerActions.AltAction.performed -= AltActionPerformed;
        _playerControls.UIActions.Pause.performed -= PausePerformed;
    }

    private void InteractionPerformed(InputAction.CallbackContext obj) {
        Debug.Log("Inter");
        playerController.interact();
    }

    private void InteractionCanceled(InputAction.CallbackContext obj) {
        playerController.interactionCancel();
    }

    private void AltActionPerformed(InputAction.CallbackContext obj) {
        Debug.Log("Alt");
        playerController.AltAction();
    }

    private void SubmitPerformed(InputAction.CallbackContext obj) {
        isSubmitPressed = true;
    }

    private void SubmitCancelled(InputAction.CallbackContext obj) {
        isSubmitPressed = false;
    }

    private void HorizontalMovementPerformed(InputAction.CallbackContext obj) {
        playerController.ReceiveHorizontalInput(obj.ReadValue<Vector2>());
    }

    private void JumpPerformed(InputAction.CallbackContext obj) {
        playerController.ReceiveJumpInput();
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

    public bool GetSubmitPressed() {
        return isSubmitPressed;
    }

    public void DisableInput() {
        _playerControls.PlayerActions.Disable();
        playerController.ReceiveHorizontalInput(new Vector2(0f, 0f));
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
