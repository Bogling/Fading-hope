using UnityEngine;

public class DoorBigManager : MonoBehaviour, IDamageable, Interactable
{
    [SerializeField] private GameObject[] activeRings;
    [SerializeField] private int startingStage;
    private BoxCollider boxCollider;
    private bool isOpened = false;
    private Animator animator;
    
    private int activeStage = 0;

    private void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        foreach (var ring in activeRings) {
            ring.SetActive(false);
        }
        activeStage = startingStage;
    }

    public void Active() {
        activeStage++;
        if (activeStage < 0 || activeStage > 2) return;
        animator.SetTrigger("Active" + activeStage);
        activeRings[activeStage - 1].SetActive(true);
    }

    public void DealDamage(float damage)
    {
        if (activeStage >= 3) {
            if (!isOpened) {
                boxCollider.enabled = false;
                activeRings[2].SetActive(true);
                animator.SetTrigger("Open");
                isOpened = true;
            }
        }
    }

    public bool IsCurrentlyInteractable()
    {
        return false;
    }

    public void Interact()
    {
        Active();
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
