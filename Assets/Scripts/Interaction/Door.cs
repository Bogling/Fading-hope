using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected Lever[] levers;
    [SerializeField] protected Animator animator;

    private bool isOpened;


    public void Interact()
    {
        if (isLocked || isOpened) return;
        if (levers.Length > 0) {
            foreach (var lever in levers) {
                if (!lever.isActivated) return;
            }
            Open();
        }
        else {
            Open();
        }
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void Open() {
        animator.SetTrigger("Open");
        isOpened = true;
    }

    public void Close() {
        animator.SetTrigger("Close");
        isOpened = false;
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public bool IsOpened() {
        return isOpened;
    }
}
