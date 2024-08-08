using UnityEngine;

public class DreamPlayerController : MonoBehaviour
{
    [SerializeField] private DreamPlayerRay playerRay;
    private Rigidbody rb;
    [Header("Movement")]
    [SerializeField] private float speed = 11f;
    [SerializeField] private float groundDrag = 11f;
    [SerializeField] private bool isJumpLocked = false;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpCooldown;
    [SerializeField] private float airMultiplier;
    private bool readyToJump = true;
    [SerializeField] private Transform orientation;
    [Header("Ground Check")]
    [SerializeField] private float playerHeight;
    [SerializeField] private LayerMask Ground;
    GameManager gameManager;

    private Vector2 horizontalInput;
    private Vector3 moveDirection;
    private bool grounded;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded) {
            rb.drag = groundDrag;
        }
        else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }
    
    public void interact() {
        Debug.Log("IntStart");
        if (playerRay.objectOnRay != null && playerRay.isInRange() && playerRay.objectOnRay.IsCurrentlyInteractable() && !DreamDialogueController.GetInstance().dialogueIsPlaying) {
            playerRay.objectOnRay.Interact();
            Debug.Log("InteractDef");
        }
        else if (!DreamDialogueController.GetInstance().dialogueIsPlaying && gameManager.GetData().hasFlashlight) {
            Debug.Log("InteractFlash");
            FindFirstObjectByType<FlashlightManager>().Flash();
        }
    }

    public void ReceiveHorizontalInput(Vector2 _horizontalInput) {
        horizontalInput = _horizontalInput;
    }

    public void ReceiveJumpInput() {
        if (isJumpLocked || !readyToJump || !grounded) return;

        readyToJump = false;
        Jump();
        Invoke(nameof(ResetJump), jumpCooldown);
    }

    private void MovePlayer() {
        Vector3 moveDirection = orientation.right * horizontalInput.x + orientation.forward * horizontalInput.y;

        if (grounded) {
            rb.AddForce(moveDirection.normalized * speed * 10f, ForceMode.Force);
        }
        else {
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
        }
    }

    private void Jump() {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }
}
