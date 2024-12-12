using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StandingTorch : MonoBehaviour, IDamageable
{
    [SerializeField] private GameObject lightPoint;
    [SerializeField] private FearArea darkArea;

    public void DealDamage(float damage) {
        lightPoint.SetActive(true);
        if (darkArea != null) {
            darkArea.Deactivate();
        }

        if (GetComponent<Lever>() != null) GetComponent<Lever>().Interact();
    }

}
