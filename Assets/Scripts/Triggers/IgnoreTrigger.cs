using UnityEngine;

public class IgnoreTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            FindFirstObjectByType<Kyle1DialogueInteraction>().CheckIgnored();
        }
    }
}
