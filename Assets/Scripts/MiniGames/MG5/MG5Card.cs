using System.Collections;
using UnityEngine;

public class MG5Card : MonoBehaviour, Interactable
{
    [SerializeField] private Sprite[] frontSprites;
    [SerializeField] private Sprite backSprite;

    [SerializeField] private SpriteRenderer frontSide;
    [SerializeField] private SpriteRenderer backSide;
    private Animator animator;

    private int type;
    private int currentIndex = 0;
    private int globalIndex = 0;
    private bool isFlipped = false;
    private bool isFound = false;

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
            StartCoroutine(MiniGame5Manager.GetInstance().OnFlipped(this));
        }
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

    public int GetCardType() {
        return type;
    }

    public void FindCard() {
        isFound = true;
        gameObject.SetActive(false);
    }

    public bool IsFound() {
        return isFound;
    }

    public IEnumerator MoveCard(Vector3 destination, float delta) {
        float inc = 0.01f;
        while (Mathf.Abs(transform.position.x - destination.x) >= delta && Mathf.Abs(transform.position.y - destination.y) >= delta && Mathf.Abs(transform.position.z - destination.z) >= delta) {
            transform.position = new Vector3(Mathf.Lerp(transform.position.x, destination.x, inc), Mathf.Lerp(transform.position.y, destination.y, inc), Mathf.Lerp(transform.position.z, destination.z, inc));
            yield return new WaitForSeconds(0.01f);
            inc += 0.01f;
        }

        transform.position = destination;
    }

    public void DestroyCard() {
        Destroy(gameObject);
    }

    public bool IsCurrentlyInteractable()
    {
        return !isFlipped && MiniGame5Manager.GetInstance().CanChooseCards() && !isFound;
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

    public static bool operator ==(MG5Card a, MG5Card b) {
        if ((object)a == null && (object)b == null) {
            return true;
        }
        
        if (((object)a == null && (object)b != null) || ((object)a != null && (object)b == null)) {
            return false;
        }

        return a.type == b.type && a.currentIndex == b.currentIndex && a.globalIndex == b.globalIndex;
    }

    public static bool operator !=(MG5Card a, MG5Card b) {
        return a.type != b.type || a.currentIndex != b.currentIndex || a.globalIndex != b.globalIndex;
    }

    public override bool Equals(object obj) {
        if (obj.GetType() == this.GetType()) {
            MG5Card other = (MG5Card)obj;
            return (currentIndex == other.currentIndex) && (globalIndex == other.globalIndex) && (type == other.type);
        }
        return false;
    }

    public override int GetHashCode() {
        return currentIndex + type;
    }
}
