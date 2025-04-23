using System.Collections;
using UnityEngine;

public class FlashableFearEvent : Day5Event
{
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private KylePendant pendant;
    [SerializeField] private float timeToLeave;
    [SerializeField] private float restTime;
    [SerializeField] private float neededPower = 1f;
    [SerializeField] private AudioSource audioSource;
    private GameManager gameManager;
    private PlayerRay playerRay;
    private Animator animator;
    private bool isWaiting = false;

    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        animator = gameObject.GetComponent<Animator>();
        playerRay = FindFirstObjectByType<PlayerRay>();
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
        return isActive;
    }

    public override bool IsResting()
    {
        return isResting;
    }

    public override void StartEvent()
    {
        animator.SetTrigger("Activate");
        Activate();
        gameManager.BlockHPRegen();
    }

    public void Activate() {
        isActive = true;
        if (audioSource != null) {
            audioSource.Play();
        }
        StartCoroutine(ManageDamage());
    }

    public void Deactivate() {
        StartCoroutine(EventRest());
        if (audioSource != null) {
            audioSource.Stop();
        }
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
                if (!isWaiting) {
                    StartCoroutine("WaitForLeave");
                }
                yield return new WaitForSeconds(checkDelay);
            }
            else {
                if (isWaiting) {
                    StopCoroutine("WaitForLeave");
                    isWaiting = false;
                }
                gameManager.DealDamage(damage);
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private IEnumerator WaitForLeave() {
        isWaiting = true;
        yield return new WaitForSeconds(timeToLeave);
        animator.SetTrigger("Deactivate");
        Deactivate();
    }

    public override void Enrage()
    {
        animator.SetBool("Enrage", true);
    }
}
