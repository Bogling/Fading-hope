using System;
using System.Collections;
using UnityEngine;

public class EyesEvent : Day5Event
{
    [SerializeField] private float[] damage;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private KylePendant pendant;
    [SerializeField] private float timeToLeave;
    [SerializeField] private float restTime;
    [SerializeField] private float neededPower = 1f;
    [SerializeField] private Material[] materials;
    [SerializeField] private Material[] emissionMaterials;
    private GameManager gameManager;
    private PlayerRay playerRay;
    private Animator animator;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer emissionMeshRenderer;
    private bool isWaiting = false;
    private int currentEyesNumber = 0;
    private bool isAppeared = false;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        playerRay = FindFirstObjectByType<PlayerRay>();
        //meshRenderer = GetComponent<MeshRenderer>();
        //emissionMeshRenderer = GetComponentInChildren<MeshRenderer>();
    }

    public override void EndEvent()
    {
        gameManager.UnblockHPRegen();
        Day5Manager.GetInstance().OnEventEnded(EventWeight);
    }

    public override IEnumerator EventRest()
    {
        isResting = true;
        yield return new WaitForSeconds(restTime);
        isResting = false;
    }

    public override bool IsActive()
    {
        return isActive && currentEyesNumber >= 5;
    }

    public override bool IsResting()
    {
        return isResting;
    }

    public override void StartEvent()
    {
        //animator.SetTrigger("Activate");
        meshRenderer.enabled = true;
        emissionMeshRenderer.enabled = true;
        Activate();
        if (isActive) {
            currentEyesNumber++;
            meshRenderer.materials = new Material[] {materials[currentEyesNumber]};
            emissionMeshRenderer.materials = new Material[] {emissionMaterials[currentEyesNumber]};
        }
    }

    public void Activate() {
        if (!isAppeared) {
            isAppeared = true;
            Day5Manager.GetInstance().OnEventEnded(EventWeight);
            meshRenderer.materials = new Material[] {materials[0]};
            emissionMeshRenderer.materials = new Material[] {emissionMaterials[0]};
        }
        else {
            if (currentEyesNumber == 0) {
                gameManager.BlockHPRegen();
                isActive = true;
                StartCoroutine(ManageDamage());
            }
        }
    }

    public void Deactivate() {
        StartCoroutine(EventRest());
        isActive = false;
        EndEvent();
    }

    private IEnumerator ManageDamage() {
        while (true) {
            if (!isActive) {
                yield break;
            }

            Ray ray = new Ray(playerRay.transform.position, playerRay.transform.forward);
            ray.direction.Normalize();
            RaycastHit raycastHit;
            Physics.Raycast(ray, out raycastHit);
            if (raycastHit.collider == gameObject.GetComponent<BoxCollider>()
            && pendant.IsUp()
            && (pendant.GetCurrentPower() >= neededPower || isWaiting)) {
                if (!isWaiting && currentEyesNumber > 0) {
                    StartCoroutine("WaitForLeave");
                }
                yield return new WaitForSeconds(checkDelay);
            }
            else {
                if (isWaiting) {
                    StopCoroutine("WaitForLeave");
                    isWaiting = false;
                }
                gameManager.DealDamage(damage[currentEyesNumber]);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private IEnumerator WaitForLeave() {
        isWaiting = true;
        yield return new WaitForSeconds(timeToLeave);
        currentEyesNumber--;
        meshRenderer.materials = new Material[] {materials[currentEyesNumber]};
        emissionMeshRenderer.materials = new Material[] {emissionMaterials[currentEyesNumber]};
        isWaiting = false;
        if (currentEyesNumber == 0) {
            Deactivate();
        }
        else {
            Day5Manager.GetInstance().OnEventEnded(EventWeight);
        }
    }

    public override void Enrage()
    {
        return;
    }
}
