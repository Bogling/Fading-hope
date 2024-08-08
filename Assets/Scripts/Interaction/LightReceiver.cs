using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightReceiver : MonoBehaviour, IDamageable
{
    public void DealDamage(float damage) {
        GetComponent<Lever>().Interact();
    }
}
