using UnityEngine;

public class Lever : MonoBehaviour, Interactable
{
    public bool isActivated = false;
    [SerializeField] private bool isTouchable = true;
    [SerializeField] private bool sendsSignal = false;
    [SerializeField] private GameObject signalDestination;
    private Animator animator;

    private void Awake() {
        animator = GetComponent<Animator>();

    }
    public void Interact()
    {
        if (!isActivated) {
            if (animator != null) {
                animator.SetTrigger("Pull");
            }
            isActivated = true;

            if (sendsSignal && signalDestination.GetComponent<Interactable>() != null) {
                signalDestination.GetComponent<Interactable>().Interact();
            }
        }
    }

    public void InteractionCanceled() {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return isTouchable;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void Reset() {
        animator.SetTrigger("Reset");
        isActivated = false;
    }

    public void Lock() {
        isTouchable = false;
    }
    
    public void Unlock() {
        isTouchable = true;
    }
}
