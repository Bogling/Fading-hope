using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private GameObject player;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            gameManager.Respawn(Color.white, 3f, true, true);
        }
    }
}
