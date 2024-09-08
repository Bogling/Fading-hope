using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CeilingLamp : MonoBehaviour, Interactable
{
    [SerializeField] private float TimeToLight;
    [SerializeField] private float TimeToFade;
    [SerializeField] private int LightTimes;
    [SerializeField] private float LoopDelay;
    [SerializeField] private bool IsDeactivating;
    [SerializeField] private CeilingLamp nextLamp;
    [SerializeField] private Animator animator;
    [SerializeField] private FearArea fearArea;

    public IEnumerator LightUp() {
        while (LightTimes != 0) {
            yield return new WaitForSeconds(TimeToLight);
            Activate();
            yield return new WaitForSeconds(TimeToFade);
            if (IsDeactivating) {
                Deactivate();
            }
            if (nextLamp != null) {
                StartCoroutine(nextLamp.LightUp());
            }
            LightTimes--;
            yield return new WaitForSeconds(LoopDelay);
        }
    }

    public void Activate() {
        animator.SetTrigger("Light");
        if (fearArea != null) {
            fearArea.Deactivate();
        }
    }

    public void Deactivate() {
        animator.SetTrigger("Fade");
        if (fearArea != null) {
            fearArea.Activate();
        }
    }

    public void Interact()
    {
        StartCoroutine(LightUp());
    }

    public bool IsCurrentlyInteractable()
    {
        return false;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }
}
