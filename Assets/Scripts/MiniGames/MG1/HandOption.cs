using UnityEngine;

public class HandOption : MonoBehaviour, Interactable
{
    [SerializeField] private bool option;
    public bool IsCurrentlyInteractable() {
        return true;
    }
 
    public void Interact() {
        MiniGame1Manager.GetInstance().SelectOption(option);
    }
    
    public void InteractionCanceled() {
        return;
    }

    public void OnHover() {
        return;
    }

    public void OnHoverStop() {
        return;
    }
}
