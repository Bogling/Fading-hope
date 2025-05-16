using System.Collections;
using UnityEngine;

public class CeilingLamp : MonoBehaviour, Interactable
{
    [SerializeField] private float TimeToLight;
    [SerializeField] private float TimeToFade;
    [SerializeField] private int LightTimes;
    [SerializeField] private float LoopDelay;
    [SerializeField] private float afterDelay;
    [SerializeField] private bool IsDeactivating;
    [SerializeField] private CeilingLamp nextLamp;
    [SerializeField] private Animator animator;
    [SerializeField] private FearArea fearArea;

    public IEnumerator LightUp() {
        int temp = LightTimes;
        while (temp != 0) {
            yield return new WaitForSeconds(TimeToLight);
            Activate();
            yield return new WaitForSeconds(TimeToFade);
            if (nextLamp != null) {
                StartCoroutine(nextLamp.LightUp());
            }
            if (IsDeactivating) {
                yield return new WaitForSeconds(afterDelay);
                Deactivate();
            }
            temp--;
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

    public void InteractionCanceled() {
        return;
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
