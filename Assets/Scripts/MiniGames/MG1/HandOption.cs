using UnityEngine;

public class HandOption : MonoBehaviour, Interactable
{
    [SerializeField] private bool option;
    public bool IsCurrentlyInteractable() {
        return true;
    }
 
    public void Interact() {
        MiniGame1Manager.GetInstance().SelectOption(option);
        //Talk(inkJSON);
    }
    
    public void InteractionCanceled() {
        return;
    }

    public void OnHover() {
        //Debug.Log("Hovered");
    }

    public void OnHoverStop() {
        Debug.Log("Released");
    }
}
