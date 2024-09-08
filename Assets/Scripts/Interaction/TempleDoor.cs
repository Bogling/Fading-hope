using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempleDoor : Door, IDamageable
{
    [SerializeField] private bool isActivated = false;
    [SerializeField] private bool isLightable = false;
    [SerializeField] private GameObject ActivationIndicator;

    public void DealDamage(float damage) {
        if (isLightable && !isActivated) {
            Activate();
        }
    }

    public void Activate() {
        ActivationIndicator.SetActive(true);
        isActivated = true;
        Open();
    }

    public void Deactivate() {
        isActivated = false;
        ActivationIndicator.SetActive(false);
    }
}
