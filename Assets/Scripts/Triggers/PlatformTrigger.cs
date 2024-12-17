using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private MovingPlatformRemote movingPlatform;
    [SerializeField] private bool triggerOnReturn = false;

    private void OnTriggerEnter(Collider collision) {
        if (collision.GetComponent<MovingPlatform>() != null) {
            if (movingPlatform.IsMovingBack()) {
                if (triggerOnReturn) {
                    movingPlatform.Activate();
                }
            }
            else {
                movingPlatform.Activate();
            }
        }
    }
}
