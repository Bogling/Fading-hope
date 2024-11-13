using UnityEngine;

public class sign : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;

    private bool isLocked = false;
    private int currentInk = 0;
    public bool IsCurrentlyInteractable() {
        if (isLocked) {
            return false;
        }
        else {
            return true;
        }
    }
 
    public void Interact() {
        Debug.Log("HobaS");
        Talk(inkJSON[currentInk]);
    }

    public void OnHover() {
        //Debug.Log("Hovered");
    }

    public void OnHoverStop() {
        Debug.Log("Released");
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

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }
    public void UponExit() {
        return;
    }
}
