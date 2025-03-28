using System.Collections;
using UnityEngine;

public class KylePendant : MonoBehaviour, Interactable
{
    [SerializeField] private GameObject pendantLight;
    [SerializeField] private float maxIntensity = 10f;
    [SerializeField] private float regenRate = 0.5f;
    [SerializeField] private float regenDelay = 0.5f;
    [SerializeField] private float minIntensity = 0f;
    [SerializeField] private float depleteRate = 0.5f;
    [SerializeField] private float depleteDelay = 0.5f;
    [SerializeField] private float smoothMoveMultiplier;
    [SerializeField] private float targetPointDistance;
    [SerializeField] private PlayerRay playerRay;
    private bool isLocked = false;
    private bool isUp = false;
    private float currentPower;
    private Vector3 currentLightPos;
    private Animator animator;

    private void Start() {    
        animator = GetComponent<Animator>();
        currentPower = maxIntensity;
        pendantLight.GetComponentInChildren<Light>().intensity = currentPower;
    }

    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            if (isUp) {
                OnDown();
            }
            else {
                OnUp();
            }
        }
    }

    public void InteractionCanceled()
    {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return !isLocked;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    private void OnUp() {
        animator.SetTrigger("Up");
        isUp = true;
        StopCoroutine(RegeneratePower());
        StartCoroutine(LightFollowCamera());
        StartCoroutine(DepletePower());
    }

    private void OnDown() {
        animator.SetTrigger("Down");
        isUp = false;
        StopCoroutine(DepletePower());
        StopCoroutine(LightFollowCamera());
        StartCoroutine(RegeneratePower());
    }

    private IEnumerator RegeneratePower() {
        while (currentPower < maxIntensity) {
            if (isUp) {
                yield break;
            }
            if (currentPower + regenRate > maxIntensity) {
                currentPower = maxIntensity;
                pendantLight.GetComponentInChildren<Light>().intensity = currentPower;
                yield break;
            }
            else {
                currentPower += regenRate;
            }
            pendantLight.GetComponentInChildren<Light>().intensity = currentPower;
            yield return new WaitForSeconds(regenDelay);
        }
    }

    private IEnumerator DepletePower() {
        while (currentPower > minIntensity) {
            if (!isUp) {
                yield break;
            }
            if (currentPower - depleteRate < minIntensity) {
                currentPower = minIntensity;
                pendantLight.GetComponentInChildren<Light>().intensity = currentPower;
                yield break;
            }
            else {
                currentPower -= depleteRate;
            }
            pendantLight.GetComponentInChildren<Light>().intensity = currentPower;
            yield return new WaitForSeconds(depleteDelay);
        }
    }

    private IEnumerator LightFollowCamera() {
        while (true) {
            Ray ray = new Ray(playerRay.transform.position, playerRay.transform.forward);
            ray.direction.Normalize();
            Vector3 point = ray.GetPoint(targetPointDistance);
            if (!isUp) {
                yield break;
            }
            Vector3 dir = (point - transform.position).normalized;
            currentLightPos = point;
            pendantLight.transform.rotation = Quaternion.Slerp(pendantLight.transform.rotation, Quaternion.LookRotation(dir), smoothMoveMultiplier * Time.deltaTime);
            yield return new WaitForSeconds(0.001f);
        }
    }
    
    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public bool IsUp() {
        return isUp;
    }

    public float GetCurrentPower() {
        return currentPower;
    }

    public void SetDepleteRate(float newRate) {
        depleteRate = newRate;
    }

    public float GetDepleteRate() {
        return depleteRate;
    }
}
