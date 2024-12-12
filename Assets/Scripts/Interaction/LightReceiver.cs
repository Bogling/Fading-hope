using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightReceiver : MonoBehaviour, IDamageable
{
    private bool isLocked = false;

    public void DealDamage(float damage) {
        if (!isLocked) {
            GetComponent<Lever>().Interact();
        }
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }
}
