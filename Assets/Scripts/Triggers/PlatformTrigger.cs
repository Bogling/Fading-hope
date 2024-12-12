using UnityEngine;

public class PlatformTrigger : MonoBehaviour
{
    [SerializeField] private MovingPlatformRemote movingPlatform;

    private void OnTriggerEnter(Collider collision) {
        if (collision.GetComponent<MovingPlatform>() != null) {
            movingPlatform.Activate();
        }
    }
}
