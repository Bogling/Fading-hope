using System.Collections;
using System.Linq;
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
    
    private void Start() {

    }

    public void Activate() {
        StartCoroutine(RandSoundLoop());
    }

    public void AddSound(AudioClip audioClip) {
        audioClips = audioClips.Append(audioClip).ToArray();
    }

    public void Deactivate() {
        isWaiting = false;
    }

    private void Update() {
    }

    private IEnumerator RandSoundLoop() {
        if (isWaiting) yield break;
        isWaiting = true;
        while(isWaiting) {
            yield return new WaitForSeconds(checkDelay);
            if ((int)Random.Range(1, chanceRange) == 1) {
                audioSource.clip = audioClips[Random.Range(0, audioClips.Length)];
                audioSource.volume = Random.Range(minVol, maxVol);
                audioSource.Play();
                yield return new WaitForSeconds(checkDelay * 10);
            }
        }
    }

    public bool IsActive() {
        return isWaiting;
    }
}
