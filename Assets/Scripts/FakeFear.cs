using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FakeFear : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Color faderColor;
    [SerializeField] private float fadeOutDuration = 1;
    [SerializeField] private float fadeInDuration = 1;
    [SerializeField] private bool fadeOut;
    [SerializeField] private bool fadeIn;
    [SerializeField] private string scene;
    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowFear() {
        gameObject.SetActive(true);
        animator.SetTrigger("Show");
    }

    public void End() {
        StartCoroutine(EndCoroutine());
    }

    public IEnumerator EndCoroutine() {
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }
        SaveLoadManager.Save();
        SceneManager.LoadScene(scene);
        if (fadeIn) {
            Fader.GetInstance().FadeIn(faderColor, fadeInDuration);
            yield return new WaitForSeconds(fadeInDuration);
        }
    }
}
