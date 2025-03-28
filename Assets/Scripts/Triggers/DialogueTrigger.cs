using UnityEngine;

public class DialogueTrigger : MonoBehaviour, ITalkable
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;
    [SerializeField] private Transform focusPoint;
    private bool isTriggered = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player && !isTriggered) {
            Talk(inkJSON);
            isTriggered = true;
        }
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
        if (focusPoint != null) {
            Focus();
        }
    }

    public void Focus() {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(focusPoint, 1.5f);
    }

    public void OperateChoice(int qID, int cID) {
        return;
    }

    public void UponExit() {
        return;
    }

    public void ChangeSprite(string spriteID) {
        return;
    }
}
