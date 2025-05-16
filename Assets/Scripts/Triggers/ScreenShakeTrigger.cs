using UnityEngine;

public class ScreenShakeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            FindFirstObjectByType<DreamPlayerCam>().ShakeScreen();
            gameObject.SetActive(false);
        }
    }
}