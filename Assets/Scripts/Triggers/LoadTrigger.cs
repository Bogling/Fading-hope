using UnityEngine;

public class LoadTrigger : MonoBehaviour
{
    [SerializeField] private GameObject[] objectsToActivate;
    [SerializeField] private GameObject[] objectsToDeactivate;
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            foreach (GameObject obj in objectsToActivate) {
                obj.SetActive(true);
            }
            foreach (GameObject obj in objectsToDeactivate) {
                obj.SetActive(false);
            }

            gameObject.SetActive(false);
        }
    }
}
