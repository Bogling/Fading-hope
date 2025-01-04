using UnityEngine;

public class DreamPlayerRay : MonoBehaviour
{

    public Interactable objectOnRay = null;

    [SerializeField] private float rayLength = 5f;

    private RaycastHit raycastHit;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position, transform.forward);
        Debug.DrawRay(transform.position, transform.forward*100f, Color.red);

        //RaycastHit raycastHit;
        Physics.Raycast(ray, out raycastHit);
        if (raycastHit.collider == null) {
            if (objectOnRay != null) {
                objectOnRay.OnHoverStop();
                objectOnRay = null;
            }
            return;
        }
        var obj = raycastHit.collider.gameObject.GetComponent<Interactable>();
        if (obj != null) {
            if (!obj.IsCurrentlyInteractable() || raycastHit.distance >= rayLength) {
                if (obj == objectOnRay) {
                    objectOnRay.OnHoverStop();
                    objectOnRay = null;
                }
                return;
            }
            if (objectOnRay != null) {
                objectOnRay.OnHoverStop();
            }
            objectOnRay = obj;
            obj.OnHover();
        }
        else if (objectOnRay != null) {
            objectOnRay.OnHoverStop();
            objectOnRay = null;
        }
    }

    public bool isInRange() {
        return raycastHit.distance <= rayLength;
    }
}
