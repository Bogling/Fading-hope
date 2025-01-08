using UnityEngine;

public class Lamp : MonoBehaviour, Interactable
{

    [SerializeField] private GameObject lamp;
    [SerializeField] private GameManager gameManager;
    [SerializeField] private MeshRenderer[] meshRenderer;
    [SerializeField] private int[] outlineIndexes;
    [SerializeField] private Material invisibleMaterial;
    [SerializeField] private Material outlineMaterial;
    private bool isHovered = false;

    private void Start() {
        for (int i = 0; i < meshRenderer.Length; i++) {
            var matArray = meshRenderer[i].materials;
            matArray[outlineIndexes[i]] = invisibleMaterial;
            meshRenderer[i].materials = matArray;
        }
    }

    public void Interact()
    {
        //GameObject lampObject = Instantiate(lamp, target.transform.position, Quaternion.identity);
        //lampObject.transform.parent = target.transform;
        //lampObject.transform.position = new Vector3(0, 0, 0);
        gameManager.GiveLamp();
        Destroy(gameObject);
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
