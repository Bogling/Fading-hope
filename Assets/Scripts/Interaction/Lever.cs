using UnityEngine;

public class Lever : MonoBehaviour, Interactable
{
    public bool isActivated = false;
    [SerializeField] private bool isTouchable = true;
    [SerializeField] private bool sendsSignal = false;
    [SerializeField] private GameObject signalDestination;
    private Animator animator;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private AudioClip pullClip;
    [SerializeField] private AudioSource audioSource;
    private bool isHovered = false;

    private void Awake() {
        animator = GetComponent<Animator>();

    }

    void Start()
    {
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }
    public void Interact()
    {
        if (!isActivated) {
            if (animator != null) {
                animator.SetTrigger("Pull");
                if (pullClip != null) {
                    audioSource.clip = pullClip;
                    audioSource.Play();
                }
            }
            isActivated = true;

            if (sendsSignal && signalDestination.GetComponent<Interactable>() != null) {
                signalDestination.GetComponent<Interactable>().Interact();
            }
        }
    }

    public void PullWithoutSignal() {
        if (!isActivated) {
            if (animator != null) {
                animator.SetTrigger("Pull");
            }
            isActivated = true;
        }
    }

    public void InteractionCanceled() {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return isTouchable;
    }

    public void OnHover()
    {
        if (!isHovered && IsCurrentlyInteractable()) {
            isHovered = true;
            for (int i = 0; i < meshRenderer.Length; i++) {
                var matArray = meshRenderer[i].materials;
                matArray[outlineIndexes[i]] = outlineMaterial;
                meshRenderer[i].materials = matArray;
            }
        }
    }

    public void OnHoverStop()
    {
        isHovered = false;
        if (!gameObject.activeInHierarchy) return;
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }

    public void Reset() {
        animator.SetTrigger("Reset");
        isActivated = false;
    }

    public void Lock() {
        isTouchable = false;
    }
    
    public void Unlock() {
        isTouchable = true;
    }

    public bool IsActivated() {
        return isActivated;
    }
}
