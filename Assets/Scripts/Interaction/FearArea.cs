using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;

public class FearArea : MonoBehaviour
{
    [SerializeField] private float damage = 10;
    [SerializeField] private float delay = 1;

    private bool isStopped = true;
    private bool isDeactivated = false;
    private GameManager gameManager;

    private void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
    }
    
    private void OnTriggerEnter(Collider other) {
        if (isDeactivated) {
            return;
        }
        isStopped = false;
        StartCoroutine(DealDamage());
    }

    private void OnTriggerExit() {
        StopDamage();
    }

    /*private async void DealDamage() {
        while (true) {
            if (isStopped) {
                gameManager.StartHPRegen();
                break;
            }
            gameManager.StopHPRegen();
            bool temp = gameManager.DealDamage(damage);
            if (temp) {
                StopDamage();
                break;
            }
            await Task.Delay((int)(delay * 1000));
        }
    } */

    private IEnumerator DealDamage() {
        while (true) {
            if (isStopped) {
                gameManager.StartHPRegen();
                break;
            }
            gameManager.StopHPRegen();
            bool temp = gameManager.DealDamage(damage);
            if (temp) {
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
        //gameManager.StartHPRegen();
    }
}
