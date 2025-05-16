using UnityEngine;

public class SignalRelay : MonoBehaviour, Interactable
{

    [SerializeField] private GameObject[] signalDestinations;

    public void Interact()
    {
        foreach (var signalDestination in signalDestinations) {
            if (signalDestination.GetComponent<Interactable>() == null) continue;
            signalDestination.GetComponent<Interactable>().Interact();
        }
    }

    public void InteractionCanceled()
    {
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
}
