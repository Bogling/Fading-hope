using System.Collections;
using UnityEngine;

public class TVEvent : Day5Event
{
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private KylePendant pendant;
    [SerializeField] private float timeToLeave;
    [SerializeField] private float restTime;
    [SerializeField] private AudioSource audioSource;
    private GameManager gameManager;
    private PlayerCam playerCam;
    private Animator animator;
    private bool isInSight;

    void Start() {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        playerCam = FindFirstObjectByType<PlayerCam>();
    }

    public override void EndEvent()
    {
        gameManager.UnblockHPRegen();
        Day5Manager.GetInstance().OnEventEnded(EventWeight);
    }

    public override void StartEvent()
    {
        animator.SetTrigger("Activate");
        Activate();
        gameManager.BlockHPRegen();
    }

    public void Activate() {
        isActive = true;
        audioSource.Play();
        StartCoroutine(ManageDamage());
    }

    private IEnumerator ManageDamage() {
        StartCoroutine("OnNotInSight");
        while (true) {
            if (!isActive) {
                yield break;
            }
            if (GeometryUtility.TestPlanesAABB(GeometryUtility.CalculateFrustumPlanes(playerCam.GetComponent<Camera>()), GetComponent<BoxCollider>().bounds)) {
                isInSight = true;
                StopCoroutine("OnNotInSight");
                gameManager.DealDamage(damage);
                yield return new WaitForSeconds(delay);
            }
            else {
                if (isInSight) {
                    isInSight = false;
                    StartCoroutine("OnNotInSight");
                }
                yield return new WaitForSeconds(checkDelay);
            }
        }
    }

    private IEnumerator OnNotInSight() {
        yield return new WaitForSeconds(timeToLeave);
        animator.SetTrigger("Deactivate");
        Deactivate();
    }

    public void Deactivate() {
        StartCoroutine(EventRest());
        audioSource.Stop();
        isActive = false;
        EndEvent();
    }

    public override IEnumerator EventRest() {
        isResting = true;
        yield return new WaitForSeconds(restTime);
        isResting = false;
    }

    public override bool IsActive()
    {
        return isActive;
    }

    public override bool IsResting()
    {
        return isResting;
    }

    public override void Enrage()
    {
        return;
    }
}
