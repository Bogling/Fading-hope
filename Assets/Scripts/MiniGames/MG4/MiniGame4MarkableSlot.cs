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
    private bool isInteractable = true;

    private bool containsPart = false;

    void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
    }

    void Start() {
        miniGame4Manager = MiniGame4Manager.GetInstance();
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
            Mark(true, false);
            miniGame4Manager.MakeTurn(true, slotPosition);
        }
    }

    public void InteractionCanceled() {
        if (miniGame4Manager.IsSelecting()) {
            miniGame4Manager.SubmitLine();
        }
    }

    public void Mark(bool isPlayer, bool isSelectMark) {
        if (isSelectMark) {
            if (isPlayer) {
                containsPart = true;
                isLocked = true;
                spriteRenderer.sprite = markedSelectSprite;
                return;
            }
            else {
                containsPart = true;
                isLocked = true;
                return;
            }
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

    public void ResetMark() {
        isMarked = false;
        containsPart = false;
        spriteRenderer.sprite = emptySprite;
    }

    public bool IsCurrentlyInteractable()
    {
        return isInteractable && !isMarked && miniGame4Manager.IsPlayersTurn() && !miniGame4Manager.IsHidden();
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

    public void Lock(bool showLock) {
        if (showLock) {
            spriteRenderer.sprite = markedLockedSprite;
        }
        isLocked = true;
    }

    public void UnLock() {
        isLocked = false;
    }

    public void Seal() {
        isInteractable = false;
    }
    
    public void UnSeal() {
        isInteractable = true;
    }

    public void MarkDead() {
        spriteRenderer.sprite = markedDeadSprite;
        isMarked = true;
    }
}
