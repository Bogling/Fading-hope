using UnityEngine;

public class FlasLightObject : MonoBehaviour, Interactable
{
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private Material invisibleMaterial;
    private bool isHovered;
    public void Interact()
    {
        FindFirstObjectByType<GameManager>().GiveFlashlight();
        gameObject.SetActive(false);
    }

    public void InteractionCanceled()
    {
        throw new System.NotImplementedException();
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
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
        if (!gameObject.activeInHierarchy) return;
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }
}
