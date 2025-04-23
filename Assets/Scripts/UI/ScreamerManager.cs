using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreamerManager : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Color faderColor;
    [SerializeField] private float fadeOutDuration = 1;
    [SerializeField] private float fadeInDuration = 1;
    [SerializeField] private bool fadeOut;
    [SerializeField] private bool fadeIn;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] clips;


    void Start()
    {
        gameObject.SetActive(false);
    }

    public void ShowScreamer(int id) {
        gameObject.SetActive(true);
        animator.SetTrigger("S" + id);
    }

    public void PlaySound(int id) {
        audioSource.clip = clips[id];
        audioSource.Play();
    }

    public void EndSignal() {
        StartCoroutine(EndScreamer());
    }

    public IEnumerator EndScreamer() {
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }
        if (FindFirstObjectByType<GameManager>().GetMaxMood() == 4) {
            SceneManager.LoadScene("Ending");
        }
        else {
            SaveLoadManager.Load();
        }
        if (fadeIn) {
            Fader.GetInstance().FadeIn(faderColor, fadeInDuration);
            yield return new WaitForSeconds(fadeInDuration);
        }
    }
}
