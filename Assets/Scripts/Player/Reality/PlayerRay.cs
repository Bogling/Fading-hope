using UnityEngine;

public class PlayerRay : MonoBehaviour
{

    public Interactable objectOnRay = null;

    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);
        if (raycastHit.collider == null) {
            return;
        }
        var obj = raycastHit.collider.gameObject.GetComponent<Interactable>();
        if (obj != null) {
            if (objectOnRay != null) {
                objectOnRay.OnHoverStop();
            }

            objectOnRay = obj;
            if (!obj.IsCurrentlyInteractable()) {
                return;
            }
            obj.OnHover();
        }
        else if (objectOnRay != null) {
            objectOnRay.OnHoverStop();
            objectOnRay = null;
        }
        
    }
}
