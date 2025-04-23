using System.Collections;
using Unity.Collections;
using UnityEngine;

public class LSMBreakEvent : Day5Event, Interactable
{
    [SerializeField] private float damage;
    [SerializeField] private float delay;
    [SerializeField] private float checkDelay;
    [SerializeField] private float restTime;
    [SerializeField] private AudioSource audioSource;
    private GameManager gameManager;
    private int repairChance = 1;

    private LSM lsm;
    void Start()
    {
        gameManager = FindFirstObjectByType<GameManager>();
        lsm = gameObject.GetComponent<LSM>();
    }

    public override void EndEvent()
    {
        gameManager.UnblockHPRegen();
        Day5Manager.GetInstance().OnEventEnded(EventWeight);
    }

    public override void StartEvent()
    {
        Activate();
        gameManager.BlockHPRegen();
        repairChance = Random.Range(0, 20);
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

    public void Interact()
    {
        if (lsm.IsBroken()) {
            TouchLSM();
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

    private void TouchLSM() {
        if (Random.Range(repairChance, 100) >= 90) {
            lsm.RepairLSM();
            Deactivate();   
        }
        else {
            repairChance += Random.Range(5, 20);
        }
    }

    public void Activate() {
        isActive = true;
        audioSource.Play();
        lsm.BreakLSM();
        StartCoroutine(ManageDamage());
    }
    public void Deactivate() {
        StartCoroutine(EventRest());
        audioSource.Stop();
        isActive = false;
        EndEvent();
    }

    private IEnumerator ManageDamage() {
        while (true) {
            if (!isActive) {
                yield break;
            }
            gameManager.DealDamage(damage);
            yield return new WaitForSeconds(delay);
        }
    }

    public override void Enrage()
    {
        return;
    }
}
