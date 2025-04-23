using UnityEngine;

public class RandomSoundTrigger : MonoBehaviour
{
    [SerializeField] private AudioClip audioClip;
    private bool isActivated = false;
    private void OnTriggerEnter(Collider other)
    {
        if (isActivated) return;
        if (!FindFirstObjectByType<RandomSoundManager>().IsActive()) {
            FindFirstObjectByType<RandomSoundManager>().Activate();
        }
        if (audioClip != null) {
            FindFirstObjectByType<RandomSoundManager>().AddSound(audioClip);
        }
        isActivated = true;
    }
}
