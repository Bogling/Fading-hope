using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputController : MonoBehaviour
{
    private PlayerControls _playerControls;

    private PlayerController playerController;

    private bool isSubmitPressed = false;

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
        _playerControls.Player.Interact.performed += InteractionPerformed;
        _playerControls.Dialogue.Submit.performed += SubmitPerformed;
        _playerControls.Dialogue.Submit.canceled += SubmitCancelled;
    }

    private void OnDisable() {
        _playerControls.Player.Interact.performed -= InteractionPerformed;
        _playerControls.Dialogue.Submit.performed -= SubmitPerformed;
        _playerControls.Dialogue.Submit.canceled -= SubmitCancelled;
    }

    private void InteractionPerformed(InputAction.CallbackContext obj) {
        playerController.interact();
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
}
