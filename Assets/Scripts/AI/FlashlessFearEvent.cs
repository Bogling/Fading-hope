using System.Collections;
using NUnit.Framework;
using Unity.Android.Gradle.Manifest;
using UnityEngine;

public class FlashlessFearEvent : Day5Event
{
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private KylePendant pendant;
    [SerializeField] private float timeToLeave;
    [SerializeField] private float restTime;
    [SerializeField] private float upSpeed;
    [SerializeField] private float upDist;
    [SerializeField] private float downSpeed;
    [SerializeField] private float downDist;
    [SerializeField] private Transform upPosition;
    [SerializeField] private Transform downPosition;
    private GameManager gameManager;
    private bool isWaiting = false;
    private bool isMovingUp = false;
    private bool isMovingDown = false;
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
        StartCoroutine(ManageDamage());
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

            if (pendant.IsUp()) {
                gameManager.DealDamage(damage);
                StartCoroutine("MoveUp");
                yield return new WaitForSeconds(delay);
            }
            else {
                StartCoroutine("MoveDown");
                yield return new WaitForSeconds(delay);
            }
        }
    }

    private IEnumerator MoveUp() {
        if (isMovingUp) {yield break;}
        isMovingUp = true;
        isMovingDown = false;
        while (isMovingUp) {
            if (gameObject.transform.position.y + upDist >= upPosition.position.y) {
                gameObject.transform.position = upPosition.position;
                yield return new WaitForSeconds(upSpeed);
            }
            else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + upDist, gameObject.transform.position.z);
                yield return new WaitForSeconds(upSpeed);
            }
        }
    }

    private IEnumerator MoveDown() {
        if (isMovingDown) {yield break;}
        isMovingDown = true;
        isMovingUp = false;
        while (isMovingDown) {
            if (gameObject.transform.position.y - downDist <= downPosition.position.y) {
                gameObject.transform.position = downPosition.position;
                StartCoroutine("WaitForLeave");
                isMovingDown = false;
                yield break;
            }
            else {
                gameObject.transform.position = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y - downDist, gameObject.transform.position.z);
                yield return new WaitForSeconds(downSpeed);
            }
        }
    }

    private IEnumerator WaitForLeave() {
        isWaiting = true;
        yield return new WaitForSeconds(timeToLeave);
        Deactivate();
    }
}
