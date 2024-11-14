using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerRay playerRay;
    private Interactable currentInteractable;
    public void interact() {
        if (playerRay.objectOnRay != null && playerRay.objectOnRay.IsCurrentlyInteractable() && !DialogueController.GetInstance().dialogueIsPlaying) {
            playerRay.objectOnRay.Interact();
            currentInteractable = playerRay.objectOnRay;
        }
    }

    public void interactionCancel() {
        if (currentInteractable != null) {
            currentInteractable.InteractionCanceled();
            currentInteractable = null;
        }
    }
}
