using UnityEngine;

public class DoorBigManager : MonoBehaviour, IDamageable, Interactable
{
    [SerializeField] private GameObject[] activeRings;
    [SerializeField] private int startingStage;
    private BoxCollider boxCollider;
    private bool isOpened = false;
    private Animator animator;
    [SerializeField] private LightReceiver lightReceiver1;
    [SerializeField] private LightReceiver lightReceiver2;
    
    private int activeStage = 0;

    private void Start() {
        animator = GetComponent<Animator>();
        boxCollider = GetComponent<BoxCollider>();
        foreach (var ring in activeRings) {
            ring.SetActive(false);
        }
        activeStage = startingStage;
    }

    public void Active() {
        activeStage++;
        if (activeStage < 0 || activeStage > 2) return;
        animator.SetTrigger("Active" + activeStage);
        activeRings[activeStage - 1].SetActive(true);
        if (lightReceiver1.GetComponent<Lever>().IsActivated()) {
            if (lightReceiver2.GetComponent<Lever>().IsActivated()) {
                FindFirstObjectByType<GameManager>().SetBigDoorStage(3);
            } else {
                FindFirstObjectByType<GameManager>().SetBigDoorStage(1);
            }
        } else {
            if (lightReceiver2.GetComponent<Lever>().IsActivated()) {
                FindFirstObjectByType<GameManager>().SetBigDoorStage(2);
            }
            else {
                FindFirstObjectByType<GameManager>().SetBigDoorStage(0);
            }
        }
        SaveLoadManager.Save();
    }

    public void DealDamage(float damage)
    {
        if (activeStage >= 2) {
            if (!isOpened) {
                boxCollider.enabled = false;
                activeRings[2].SetActive(true);
                animator.SetTrigger("Open");
                isOpened = true;
                FindFirstObjectByType<GameManager>().SetBigDoorStage(4);
                SaveLoadManager.Save();
            }
        }
    }

    public bool IsCurrentlyInteractable()
    {
        return false;
    }

    public void Interact()
    {
        Active();
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void InteractionCanceled()
    {
        return;
    }
}
