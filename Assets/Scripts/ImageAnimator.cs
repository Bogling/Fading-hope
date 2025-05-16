using System;
using UnityEngine;

public class ImageAnimator : MonoBehaviour
{
    private MeshRenderer meshRenderer;
    [SerializeField] private MeshRenderer additionalMeshrenderer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
    }

    public void ChangeTexture(Texture2D texture) {
        meshRenderer.material.SetTexture("_MainTex", texture);
    }

    public void ChangeTextures(TextureList textureList) {
        meshRenderer.material.SetTexture("_MainTex", textureList.texture1);
        additionalMeshrenderer.material.SetTexture("_BaseMap", textureList.texture2);
    }
}
