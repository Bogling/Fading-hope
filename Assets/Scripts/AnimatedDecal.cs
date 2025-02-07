using System.Collections;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class AnimatedDecal : MonoBehaviour
{
    [SerializeField] private Texture[] textures;
    [SerializeField] private bool animateFromStart;
    [SerializeField] private float timeForFrame;
    [SerializeField] private Texture idleTexture;
    private DecalProjector decalProjector;
    private bool isAnimating = false;
    private int currentFrame = 0;
    private IEnumerator activeCoroutine;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        decalProjector = GetComponent<DecalProjector>();
        if (animateFromStart) {
            StartAnimation();
        }
        else if (idleTexture != null) {
            decalProjector.material.SetTexture("Base_Map", idleTexture);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartAnimation() {
        if (!isAnimating) {
            isAnimating = true;
            activeCoroutine = AnimateDecal();
            StartCoroutine(activeCoroutine);
        }
    }

    public void StopAnimation() {
        if (isAnimating) {
            isAnimating = false;
            StopCoroutine(activeCoroutine);
        }

        if (idleTexture != null) {
            decalProjector.material.SetTexture("Base_Map", idleTexture);
        }
    }

    private IEnumerator AnimateDecal() {
        while (isAnimating) {
            decalProjector.material.SetTexture("Base_Map", textures[currentFrame]);
            yield return new WaitForSeconds(timeForFrame);

            if (currentFrame + 1 >= textures.Length) {
                currentFrame = 0;
            }
            else {
                currentFrame++;
            }
        }
    }
}
