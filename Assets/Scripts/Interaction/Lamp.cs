using UnityEngine;

public class Lamp : MonoBehaviour, Interactable
{

    [SerializeField] private GameObject lamp;
    [SerializeField] private GameManager gameManager;
    public void Interact()
    {
        //GameObject lampObject = Instantiate(lamp, target.transform.position, Quaternion.identity);
        //lampObject.transform.parent = target.transform;
        //lampObject.transform.position = new Vector3(0, 0, 0);
        gameManager.GiveLamp();
        Destroy(gameObject);
    }

    public void InteractionCanceled() {
        return;
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
}
