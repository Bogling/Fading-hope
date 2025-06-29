using UnityEngine;

public class MovingPlatformRemote : MovingPlatform, Interactable
{
    [SerializeField] private bool isLocked = false;
    [SerializeField] private Lever[] levers;
    [SerializeField] private bool deactivateLevers;

    private bool isActivated;

    public void Interact()
    {
        if (isLocked || isActivated) return;
        if (levers.Length > 0) {
            foreach (var lever in levers) {
                if (!lever.isActivated) return;
            }
            Activate();
        }
        else {
            Activate();
        }
    }
    
    public void InteractionCanceled() {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return false;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }
    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public bool IsOpened() {
        return isActivated;
    }

    public override void Deactivate() {
        base.Deactivate();
        foreach (var lever in levers) {
            lever.Reset();
        }
    }
}
