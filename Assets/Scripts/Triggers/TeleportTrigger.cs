using System.Collections;
using UnityEngine;

public class TeleportTrigger : MonoBehaviour
{
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject player;
    [SerializeField] private Transform teleportPosition;
    [SerializeField] private bool fadeOut;
    [SerializeField] private Color faderColor;
    [SerializeField] private float fadeOutDuration;
    [SerializeField] private float fadeInDuration;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == playerObject) {
            StartCoroutine(Teleport());
        }
    }

    private IEnumerator Teleport() {
        if (fadeOut) {
            Fader.GetInstance().FadeOut(faderColor, fadeOutDuration);
            yield return new WaitForSeconds(fadeOutDuration);
        }

        player.gameObject.transform.position = teleportPosition.position;
        player.gameObject.GetComponent<Rigidbody>().position = teleportPosition.position;

        if (fadeOut) {
            Fader.GetInstance().FadeIn(faderColor, fadeInDuration);
            yield return new WaitForSeconds(fadeInDuration);
        }
    }
}
