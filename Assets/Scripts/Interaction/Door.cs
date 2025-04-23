using UnityEngine;

public class Door : MonoBehaviour, Interactable
{
    [SerializeField] protected bool isLocked = false;
    [SerializeField] protected Lever[] levers;
    [SerializeField] protected Animator animator;

    private bool isOpened;
    [SerializeField] private SkinnedMeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private AudioClip doorOpenClip;
    [SerializeField] private AudioClip doorCloseClip;
    [SerializeField] private AudioSource audioSource;
    private bool isHovered = false;


    public void Interact()
    {
        if (isLocked || isOpened) return;
        if (levers.Length > 0) {
            foreach (var lever in levers) {
                if (!lever.isActivated) return;
            }
            Open();
        }
        else {
            Open();
        }
    }

    public void InstaOpen() {
        if (isLocked || isOpened) return;
        animator.SetTrigger("InstaOpen");
        isOpened = true;
    }

    public void InteractionCanceled() {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
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

    public void Open() {
        animator.SetTrigger("Open");
        isOpened = true;
        audioSource.clip = doorOpenClip;
        audioSource.Play();
    }

    public void Close() {
        animator.SetTrigger("Close");
        isOpened = false;
        audioSource.clip = doorCloseClip;
        audioSource.Play();
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public bool IsOpened() {
        return isOpened;
    }
}
