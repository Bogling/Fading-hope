using UnityEngine;

public class ChangeAmbientTrigger : MonoBehaviour
{
    [SerializeField] private AudioSource ambient;
    [SerializeField] private GameObject player;
    [SerializeField] private AudioClip audioClip;
    private bool isTriggered = false;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player && !isTriggered) {
            ambient.Stop();
            ambient.clip = audioClip;
            if (audioClip != null) {
                ambient.Play();
            }
            isTriggered = true;
        }
    }
}
