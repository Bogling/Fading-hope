using System.Threading.Tasks;
using UnityEngine;

public class RandomSoundManager : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float checkDelay;
    [SerializeField] private float chanceRange;

    [SerializeField] private float minVol;
    [SerializeField] private float maxVol;

    private AudioSource audioSource;
    private bool isWaiting = false;

    private void Awake() {
        audioSource = GetComponent<AudioSource>();
    }

    private async void Update() {
        if (isWaiting) return;
        isWaiting = true;
        await Task.Delay((int)(checkDelay * 1000));
        if (Random.Range(1, chanceRange) == 1) {
            audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
            audioSource.volume = Random.Range(minVol, maxVol);
            audioSource.Play();
        }
        isWaiting = false;
    }
}
