using System.Collections;
using UnityEngine;

public class HandFearEvent : Day5Event, Interactable
{
    [SerializeField] private float damage;
    [SerializeField] private float baseLPred;
    [SerializeField] private float LPstep;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private KylePendant pendant;
    [SerializeField] private float timeToLeave;
    [SerializeField] private float restTime;
    [SerializeField] private float upSpeed;
    [SerializeField] private float upDist;
    [SerializeField] private float downDist;
    [SerializeField] private Transform upPosition;
    [SerializeField] private Transform downPosition;
    [SerializeField] private AudioSource audioSource;
    private GameManager gameManager;
    private bool isWaiting = false;
    private bool isMovingUp = false;
    private float currentModifier = 0f;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
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

    public void Interact()
    {
        if (isActive) {
            TouchHand();
        }
    }

    public void InteractionCanceled()
    {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    private void TouchHand() {
        if (transform.position.y - downDist <= downPosition.position.y) {
            isMovingUp = false;
            pendant.SetDepleteRate(pendant.GetDepleteRate() - currentModifier);
            currentModifier = 0;
            Deactivate();   
        }
        else {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - downDist, gameObject.transform.position.z);
            currentModifier -= LPstep * downDist / upDist;
            pendant.SetDepleteRate(pendant.GetDepleteRate() - LPstep * downDist / upDist);
        }
    }

    private IEnumerator ManageDamage() {
        while (true) {
            if (!isActive) {
                yield break;
            }
            StartCoroutine("MoveUp");
            gameManager.DealDamage(damage);
            yield return new WaitForSeconds(delay);
        }
    }

    private IEnumerator MoveUp() {
        if (isMovingUp) {yield break;}
        isMovingUp = true;
        currentModifier = baseLPred;
        pendant.SetDepleteRate(pendant.GetDepleteRate() + baseLPred);
        while (isMovingUp) {
            if (gameObject.transform.position.y + upDist >= upPosition.position.y) {
                gameObject.transform.position = upPosition.position;
                yield return new WaitForSeconds(upSpeed);
            }
            else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + upDist, gameObject.transform.position.z);
                currentModifier += LPstep;
                pendant.SetDepleteRate(pendant.GetDepleteRate() + LPstep);
                yield return new WaitForSeconds(upSpeed);
            }
        }
    }

    public override void Enrage()
    {
        return;
    }
}
