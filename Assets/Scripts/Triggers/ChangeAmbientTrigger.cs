using System.Collections;
using UnityEngine;

public class ChangeAmbientTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource ambient;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private bool isInstant = true;
    [SerializeField] private float fadeInStep = 0.01f;
    [SerializeField] private float fadeOutStep = 0.01f;
    [SerializeField] private float finalVolume = 1f;
    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player && !isTriggered) {
            StopCoroutine("FadeAmbient");
            if (isInstant) {
                bool toPlay = false;
                if (!ambient.clip == audioClip) {
                    ambient.Stop();
                    ambient.clip = audioClip;
                    toPlay = true;
                }
                if (audioClip != null) {
                    ambient.volume = finalVolume;
                    if (toPlay) {
                        ambient.Play();
                    }
                }
            }
            else {
                StartCoroutine("FadeAmbient");
            }
            isTriggered = true;
        }
    }

    private IEnumerator FadeAmbient() {
        while (ambient.volume > 0) {
            ambient.volume -= fadeInStep;
            yield return new WaitForSeconds(0.01f);
        }
        if (ambient.volume < 0) {
            ambient.volume = 0;
        }
        bool toPlay = false;
        if (!ambient.clip == audioClip) {
            ambient.Stop();
            ambient.clip = audioClip;
            toPlay = true;
        }
        if (audioClip != null && toPlay) {
            ambient.Play();
        }
        else {
            yield break;
        }
        while (ambient.volume < finalVolume) {
            ambient.volume += fadeOutStep;
            yield return new WaitForSeconds(0.01f);
        }
        if (ambient.volume > finalVolume) {
            ambient.volume = finalVolume;
        }
    }
}
