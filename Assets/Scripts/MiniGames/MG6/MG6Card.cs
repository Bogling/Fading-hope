using System.Collections;
using UnityEngine;

public class MG6Card : MonoBehaviour, Interactable
{

    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite backSprite;
    [SerializeField] private GameObject container;
    [SerializeField] private SpriteRenderer frontSide;
    [SerializeField] private SpriteRenderer backSide;
    
    private MG6Deck assignedDeck;

    private Animator animator;

    private bool isHoveredOver = false;
    private int type;
    private int currentIndex = 0;
    private int globalIndex = 0;

    private bool isMoving = false;
    private bool pendingDestroy = false;

    private void Awake() {
        animator = GetComponent<Animator>();
    }

    public void AssignDeck(MG6Deck deck) {
        assignedDeck = deck;
    }

    public void SetCard(int type, int index, int globalIndex) {
        this.type = type;
        currentIndex = index;
        this.globalIndex = globalIndex;
        frontSide.sprite = frontSprites[currentIndex];
        backSide.sprite = backSprite;
    }

    public int GetIndex() {
        return currentIndex;
    }

    public int GetGlobalIndex() {
        return globalIndex;
    }

    public bool IsMoving() { return isMoving; }

    public IEnumerator MoveCard(Vector3 destination, float delta) {
        //StopCoroutine("MoveCard");
        isMoving = true;
        float inc = 0.01f;
        while (Mathf.Abs(transform.position.x - destination.x) >= delta && Mathf.Abs(transform.position.y - destination.y) >= delta && Mathf.Abs(transform.position.z - destination.z) >= delta) {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, destination.x, inc), Mathf.Lerp(transform.position.y, destination.y, inc), Mathf.Lerp(transform.position.z, destination.z, inc));
            yield return new WaitForSeconds(0.01f);
            inc += 0.01f;
        }

        transform.position = destination;
        SetOrder();
        isMoving = false;
        if (pendingDestroy) {
            Destroy(gameObject);
        }
        yield break;
    }

    public void RotateCard(Vector3 destination) {
        transform.rotation = Quaternion.Euler(destination);
    }


    public void SelectCard() {
        animator.SetTrigger("Select");

    }

    public void UnSelectCard() {
        animator.SetTrigger("Unselect");
    }

    public void ToDefault() {
        animator.SetTrigger("ToDefault");
    }

    public void DestroyCard() {
        gameObject.SetActive(false);
        if (isHoveredOver || isMoving) {
            pendingDestroy = true;
        }
        else {
            Destroy(gameObject);
        }
    }

    public void SetOrder() {
        frontSide.sortingOrder = (int)Mathf.Abs(container.transform.position.z * 1000);
        backSide.sortingOrder = (int)Mathf.Abs(container.transform.position.z * 1000);
    }

    public bool IsCurrentlyInteractable()
    {
        if (assignedDeck == null) return false;
        return !isMoving && assignedDeck.CanMakeSelection() && !assignedDeck.IsLocked();
    }

    public void Interact()
    {
        if (assignedDeck != null) {
            assignedDeck.OnCardInteraction(this, true);
        }
    }

    public void PseudoInteract() {
        if (assignedDeck != null) {
            assignedDeck.OnCardInteraction(this, false);
        }
    }

    public void OnHover()
    {
        if (IsCurrentlyInteractable() && !isHoveredOver) {
            TranslateCard(0.05f);
            isHoveredOver = true;
        }
    }

    public void OnHoverStop()
    {
        if (pendingDestroy) {
            Destroy(gameObject);
        }
        
        if (isHoveredOver) {
            ReturnCard();
            isHoveredOver = false;
        }
    }

    private void TranslateCard(float dif) {
        Vector3 a = assignedDeck.gameObject.transform.forward;
        if (a.x == 1) {
            container.transform.position = new Vector3(container.transform.position.x + dif, container.transform.position.y, container.transform.position.z);
        }
        else if (a.y == 1) {
            container.transform.position = new Vector3(container.transform.position.x, container.transform.position.y + dif, container.transform.position.z);
        }
        else {
            container.transform.position = new Vector3(container.transform.position.x, container.transform.position.y, container.transform.position.z + dif);
        }
        SetOrder();
    }

    private void ReturnCard() {
        container.transform.position = transform.position;
        SetOrder();
    }

    public void InteractionCanceled()
    {
        return;
    }
}
