using UnityEngine;

public class MiniGame4MarkableSlot : MonoBehaviour, Interactable
{
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private Sprite markedSprite;
    [SerializeField] private Sprite markedShotSprite;
    [SerializeField] private Sprite markedDeadSprite;
    [SerializeField] private Sprite markedSelectSprite;
    [SerializeField] private Sprite markedLockedSprite;
    private SlotPosition slotPosition;
    private SpriteRenderer spriteRenderer;
    private MiniGame4Manager miniGame4Manager;
    private bool isMarked = false;
    private bool isLocked = false;

    private bool containsPart = false;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        miniGame4Manager = MiniGame4Manager.GetInstance();
        spriteRenderer.sprite = emptySprite;
    }

    public bool IsMarked() {
        return isMarked;
    }

    public bool IsLocked() {
        return isLocked;
    }

    public bool ContainsPart() {
        return containsPart;
    }

    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            if (miniGame4Manager.CanSelect()) {
                if (!isLocked && !isMarked) {
                    miniGame4Manager.SelectForShipPlacement(slotPosition);
                }
                return;
            }
            if (containsPart) {
                isMarked = true;
                spriteRenderer.sprite = markedShotSprite;
            }
            else {
                isMarked = true;
                spriteRenderer.sprite = markedSprite;
            }
        }
    }

    public void InteractionCanceled() {
        if (miniGame4Manager.IsSelecting()) {
            miniGame4Manager.SubmitLine();
        }
    }

    public void Mark(bool isPlayer, bool isSelectMark) {
        if (isSelectMark) {
            containsPart = true;
            isLocked = true;
            spriteRenderer.sprite = markedSelectSprite;
            return;
        }

        if (IsCurrentlyInteractable()) {
            if (containsPart) {
                isMarked = true;
                spriteRenderer.sprite = markedShotSprite;
                miniGame4Manager.MakeTurn(isPlayer);
            }
            else {
                isMarked = true;
                spriteRenderer.sprite = markedSprite;
                miniGame4Manager.MakeTurn(isPlayer);
            }
        }
    }

    public void ResetMark() {
        isMarked = false;
        spriteRenderer.sprite = emptySprite;
    }

    public bool IsCurrentlyInteractable()
    {
        return !isMarked && miniGame4Manager.IsPlayersTurn();
    }

    public void OnHover()
    {
        if (miniGame4Manager.IsSelecting()) {
            miniGame4Manager.TraceLine(slotPosition);
        }
    }

    public void OnHoverStop()
    {
        return;
    }    

    public void SetIndex(int xIndex, int yIndex) {
        slotPosition = new SlotPosition(xIndex, yIndex);
    }

    public SlotPosition GetSlotPosition() {
        return slotPosition;
    }
    
    public void Select() {
        spriteRenderer.sprite = markedSelectSprite;
    }

    public void UnSelect() {
        if (!isMarked) {
            spriteRenderer.sprite = emptySprite;
        }
        else {
            spriteRenderer.sprite = markedLockedSprite;
        }
    }

    public void Lock() {
        spriteRenderer.sprite = markedLockedSprite;
        isLocked = true;
    }
}
