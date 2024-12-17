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
        throw new System.NotImplementedException();
    }

    public bool IsCurrentlyInteractable()
    {
        throw new System.NotImplementedException();
    }

    public void OnHover()
    {
        throw new System.NotImplementedException();
    }

    public void OnHoverStop()
    {
        throw new System.NotImplementedException();
    }
}
