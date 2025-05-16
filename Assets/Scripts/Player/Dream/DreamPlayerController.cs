using UnityEngine;

public class DreamPlayerController : MonoBehaviour
{
    [SerializeField] private DreamPlayerRay playerRay;
    private Rigidbody rb;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource jumpAudioSource;
    private StepSoundManager stepSoundManager;
    [Header("Movement")]
    [SerializeField] private float speed = 11f;
    [SerializeField] private float groundDrag = 11f;
    [SerializeField] private float airDrag = 5f;
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

    [Header("Player Step Climb")]
    [SerializeField] private float stepHeight = 0.3f;
    [SerializeField] private float stepSmooth = 0.1f;
    [SerializeField] private float stepDistL = 0.2f;
    [SerializeField] private float stepDistH = 0.3f;
    [SerializeField] private float stepVal = 0.1f;
    private Vector2 horizontalInput;
    private bool grounded;
    private Interactable currentInteractable;

    private void Start() {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        stepSoundManager = GetComponent<StepSoundManager>();
        gameManager = FindFirstObjectByType<GameManager>();
    }

    private void Update() {
        grounded = Physics.Raycast(transform.position, Vector3.down, playerHeight * 0.5f + 0.2f, Ground);

        if (grounded) {
            rb.linearDamping = groundDrag;
        }
        else {
            rb.linearDamping = airDrag;
        }
    }

    private void FixedUpdate() {
        MovePlayer();
    }
    
    public void interact() {
        if (playerRay.objectOnRay != null && playerRay.isInRange() && playerRay.objectOnRay.IsCurrentlyInteractable() && !DreamDialogueController.GetInstance().dialogueIsPlaying) {
            playerRay.objectOnRay.Interact();
            currentInteractable = playerRay.objectOnRay;
        }
        else if (!DreamDialogueController.GetInstance().dialogueIsPlaying && gameManager.GetData().hasFlashlight) {
            FindFirstObjectByType<FlashlightManager>().Flash();
        }
    }

    public void interactionCancel() {
        if (currentInteractable != null) {
            currentInteractable.InteractionCanceled();
            currentInteractable = null;
        }
    }

    public void AltAction() {
        if (!DreamDialogueController.GetInstance().dialogueIsPlaying && gameManager.GetData().hasFlashlight) {
            FindFirstObjectByType<FlashlightManager>().ToggleFlashlight();
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
            if (moveDirection != Vector3.zero) {
                stepSoundManager.CheckGround();
                if (!audioSource.isPlaying) {
                    audioSource.pitch = Random.Range(0.85f, 1.1f);
                    audioSource.Play();
                }
            }
            else {
                audioSource.Stop();
            }
        }
        else {
            rb.AddForce(moveDirection.normalized * speed * 10f * airMultiplier, ForceMode.Force);
            audioSource.Stop();
        }
    }

    private void Jump() {
        rb.linearVelocity = new Vector3(rb.linearVelocity.x, 0f, rb.linearVelocity.z);
        stepSoundManager.CheckGround();
        jumpAudioSource.pitch = Random.Range(0.85f, 1.1f);
        jumpAudioSource.Play();
        rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);
    }

    private void ResetJump() {
        readyToJump = true;
    }
}
