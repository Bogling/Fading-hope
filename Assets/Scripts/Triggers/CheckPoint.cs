using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    [SerializeField] private Transform point;
    [SerializeField] private bool withLamp;
    [SerializeField] private bool withFlashlight;
    [SerializeField] private bool hasHP;
    [SerializeField] private bool hasLP;
    public int id;
    private GameManager gameManager;

    private void Awake() {
        gameManager = FindFirstObjectByType<GameManager>();
    }

    public Transform getPoint() {
        return point;
    }

    private void OnTriggerEnter(Collider other) {
        gameManager.SetCheckPoint(id, withLamp, withFlashlight, hasHP, hasLP);
        SaveLoadManager.Save();
    }
}
