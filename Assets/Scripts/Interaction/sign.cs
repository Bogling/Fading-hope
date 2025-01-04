using UnityEngine;

public class sign : MonoBehaviour, Interactable, ITalkable
{
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float focusSpeed;

    private bool isLocked = false;
    private int currentInk = 0;
    private bool isHovered = false;

    private void Start() {
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }

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

    public void InteractionCanceled() {
        return;
    }

    public void OnHover() {
        if (!isHovered) {
            isHovered = true;
            for (int i = 0; i < meshRenderer.Length; i++) {
                var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = outlineMaterial;
            meshRenderer[i].materials = matArray;
            }
        }
    }

    public void OnHoverStop() {
        isHovered = false;
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(focusPosition, focusSpeed);
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
