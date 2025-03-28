using UnityEngine;

public class testInteraction : MonoBehaviour, Interactable, ITalkable
{

    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;

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
        Debug.Log("Hoba");
        Talk(inkJSON[currentInk]);
    }

    public void InteractionCanceled() {
        return;
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

    public void ChangeSprite(string spriteID) {
        return;
    }

    public void Focus() {

    }
    
    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes");
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        break;
                }
                break;
            case 1:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes1");
                        MiniGame1Manager.GetInstance().StartMiniGame();
                        isLocked = true;
                        currentInk++;
                        break;
                    case 1:
                        Debug.Log("Answer is no1");
                        break;
                }
                break;
        }
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
