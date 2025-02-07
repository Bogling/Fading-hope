using System.Collections;
using UnityEngine;

public class Radio : MonoBehaviour, Interactable
{
    [SerializeField] private int checksCount = 1;
    [SerializeField] private AudioClip checkClip;
    [SerializeField] private AudioClip finalClip;
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material invisibleMaterial;
    private bool isHovered = false;
    private bool isTurnedOn = false;
    private int turnCount = 0;

    private AudioSource audioSource;
    private void Start() {
       audioSource = gameObject.GetComponent<AudioSource>(); 
    }

    public void Interact()
    {
        if (IsCurrentlyInteractable()) {
            turnCount++;
            if (turnCount < checksCount) {
                audioSource.clip = checkClip;
                audioSource.Play();
                StartCoroutine(PlayRadio());
            }
            else {
                audioSource.clip = finalClip;
                audioSource.Play();
                StartCoroutine(PlayRadio());
            }
        }
    }

    public void InteractionCanceled()
    {
        return;
    }

    public bool IsCurrentlyInteractable()
    {
        if (isTurnedOn) {
            return false;
        }
        if (turnCount <= checksCount) {
            return true;
        }
        else {
             return false;
        }
    }

    public void OnHover()
    {
        if (!isHovered) {
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
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }

    private IEnumerator PlayRadio() {
        isTurnedOn = true;
        while (!audioSource.isPlaying) {
            yield return new WaitForEndOfFrame();
        }
        isTurnedOn = false;
    }
}
