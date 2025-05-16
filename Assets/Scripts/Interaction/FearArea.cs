using System.Collections;
using UnityEngine;

public class FearArea : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float delay = 1;

    [SerializeField] private GameObject player;
    [SerializeField] private bool isFake = false;
    private bool isPlayerInRange = false;
    private bool isStopped = true;
    private bool isDeactivated = false;
    private GameManager gameManager;
    

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject != player) return;
        isPlayerInRange = true;
        if (isDeactivated) {
            return;
        }
        isStopped = false;
        StartCoroutine(DealDamage());
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject != player) return;
        isPlayerInRange = false;
        StopDamage();
    }

    private IEnumerator DealDamage() {
        while (true) {
            if (isStopped) {
                break;
            }
            if (gameManager.GetHP() - damage <= 0 && isFake) {
                StopDamage();
                break;
            }
            int temp = gameManager.DealDamage(damage);
            if (temp == 1 || (temp == 0 && isFake)) {
                StopDamage();
                break;
            }
            yield return new WaitForSeconds(delay);
        }
    }
    
    public void StopDamage() {
        isStopped = true;
    }

    public void Deactivate() {
        isDeactivated = true;
        StopDamage();
    }

    public void Activate() {
        isDeactivated = false;
        if (isPlayerInRange) {
            isStopped = false;
            StartCoroutine(DealDamage());
        }
    }
}
