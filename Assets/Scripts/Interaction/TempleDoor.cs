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
            if (levers.Length > 0) {
                foreach (var lever in levers) {
                    if (!lever.isActivated) return;
                }
                Activate();
            }
            else {
                Activate();
            }
        }
    }

    public void Activate() {
        ActivationIndicator.SetActive(true);
        isActivated = true;
        Open();
    }

    public override bool IsCurrentlyInteractable()
    {
        return false;
    }

    

    public override void Interact()
    {
        if (levers.Length > 0) {
            foreach (var lever in levers) {
                if (!lever.isActivated) return;
            }
            Activate();
        }
        else {
            Activate();
        }
    }

    public void Deactivate() {
        isActivated = false;
        ActivationIndicator.SetActive(false);
    }

    public void MakeUnlightable() {
        isLightable = false;
    }
}
