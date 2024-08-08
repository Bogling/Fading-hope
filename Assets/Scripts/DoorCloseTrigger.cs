using UnityEngine;

public class DoorCloseTrigger : MonoBehaviour
{
    [SerializeField] private Door door;

    [SerializeField] private GameObject triggerSource;
    [SerializeField] private bool closeForever = false;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == triggerSource) {
            if (door.IsOpened()) {
                door.Close();
            }
            if (closeForever) {
                door.Lock();
            }
        }
    }
}
