using UnityEngine;

public class Day2WindowDialogue : BasicDialogueScript
{
    [SerializeField] private Day2DialogueManager day2DialogueManager;
    public override void UponExit() {
        day2DialogueManager.Activate();
        day2DialogueManager.Interact();
        return;
    }
}
