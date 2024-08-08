public interface Interactable
{
    // Allows interactables to decide if they are currently interactable
    bool IsCurrentlyInteractable();
 
    // What happens when the player interacts with this interactable?
    void Interact();

    // What happens when player hovers over interactable
    void OnHover();

    // What happens when player stops hovering over interactable
    void OnHoverStop();
}
