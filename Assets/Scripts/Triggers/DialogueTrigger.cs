using UnityEngine;

public class DialogueTrigger : MonoBehaviour, ITalkable
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;
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
    }

    public void Focus() {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(transform, 2);
    }

    public void OperateChoice(int qID, int cID) {
        return;
    }

    public void UponExit() {
        return;
    }
}
