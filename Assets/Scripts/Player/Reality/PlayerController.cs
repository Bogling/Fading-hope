using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private PlayerRay playerRay;
    public void interact() {
        if (playerRay.objectOnRay != null && playerRay.objectOnRay.IsCurrentlyInteractable() && !DialogueController.GetInstance().dialogueIsPlaying) {
            playerRay.objectOnRay.Interact();
        }
    }
}
