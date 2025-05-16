public interface Interactable
{
    bool IsCurrentlyInteractable();
 
    void Interact();

    void OnHover();

    void OnHoverStop();

    void InteractionCanceled();
}
