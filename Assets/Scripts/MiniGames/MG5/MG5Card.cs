using UnityEngine;

public class MG5Card : MonoBehaviour, Interactable
{
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite backSprite;

    [SerializeField] private SpriteRenderer frontSide;
    [SerializeField] private SpriteRenderer backSide;
    private Animator animator;

    private int currentIndex = 0;
    private bool isFlipped = false;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void Flip() {
        if (isFlipped) {
            animator.SetTrigger("FlipBack");
            isFlipped = false;
        }
        else {
            isFlipped = true;
            animator.SetTrigger("Flip");
        }
    }

    public void SetIndex() {
        frontSide.sprite = frontSprites[currentIndex];
    }

    public bool IsCurrentlyInteractable()
    {
        return !isFlipped;
    }

    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            Flip();
        }
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void InteractionCanceled()
    {
        return;
    }
}
