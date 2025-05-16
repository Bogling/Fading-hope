using UnityEngine;

public class KillZone : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Color resColor = Color.white;
    [SerializeField] private float fadeInDuration = 3f;
    [SerializeField] private bool fadeIn;
    [SerializeField] private float fadeOutDuration = 3f;
    [SerializeField] private bool fadeOut;
    [SerializeField] private bool isGameOver = false;
    private GameManager gameManager;

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            if (isGameOver) {
                StartCoroutine(gameManager.CallGameOver(resColor, fadeInDuration, fadeIn, fadeOut));
            }
            else {
                StartCoroutine(gameManager.Respawn(resColor, fadeInDuration, fadeIn, fadeOutDuration, fadeOut));
            }
        }
    }
}
