using UnityEngine;

public class BasicDialogueScript : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float focusSpeed;

    private bool isLocked = false;
    private int currentInk = 0;
    private bool isHovered = false;

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
        if (currentInk + 1 < inkJSON.Length) {
            currentInk++;
        }
    }

    public void InteractionCanceled() {
        return;
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(focusPosition, focusSpeed);
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
    public virtual void UponExit() {
        return;
    }

    public void ChangeSprite(string spriteID) {
        return;
    }

    public void OnHover() {
        if (!isHovered) {
            isHovered = true;
            var matArray = meshRenderer.materials;
            matArray[0] = outlineMaterial;
            meshRenderer.materials = matArray;
        }
    }

    public void OnHoverStop() {
        isHovered = false;
        var matArray = meshRenderer.materials;
        matArray[0] = invisibleMaterial;
        meshRenderer.materials = matArray;
    }
}
