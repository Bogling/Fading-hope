using UnityEngine;

public class MarkableSlot : MonoBehaviour, Interactable
{
    private bool isMarked = false;
    private int mark = 0;
    [SerializeField] private Sprite xSprite;
    [SerializeField] private Sprite oSprite;
    [SerializeField] private Sprite emptySprite;
    [SerializeField] private int xIndex;
    [SerializeField] private int yIndex;
    private MiniGame3Manager miniGame3Manager;
    private SpriteRenderer spriteRenderer;

    void Awake() {
        miniGame3Manager = FindFirstObjectByType<MiniGame3Manager>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = emptySprite;
    }

    public int GetMark() {
        return mark;
    }

    public bool IsMarked() {
        return isMarked;
    }
    
    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            Mark(true);
        }
    }

    public void InteractionCanceled() {
        return;
    }

    public void Mark(bool isPlayer) {
        if (isPlayer) {
            spriteRenderer.sprite = xSprite;
            isMarked = true;
            mark = 1;
            miniGame3Manager.MakeTurn(true, xIndex, yIndex);
        }
        else {
            spriteRenderer.sprite = oSprite;
            isMarked = true;
            mark = -1;
            miniGame3Manager.MakeTurn(false, xIndex, yIndex);
        }
    }

    public bool IsCurrentlyInteractable()
    {
        return miniGame3Manager.GetPlayersTurn() && !isMarked;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void ResetMark() {
        isMarked = false;
        mark = 0;
        spriteRenderer.sprite = emptySprite;
    }
}
