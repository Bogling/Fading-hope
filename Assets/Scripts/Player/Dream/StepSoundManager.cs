using System.Collections.Generic;
using UnityEngine;

public class StepSoundManager : MonoBehaviour
{
    [SerializeField] private List<string> stepSoundsNames;
    [SerializeField] private List<AudioClip> stepSoundsList;
    private Dictionary<string, AudioClip> stepSounds = new Dictionary<string, AudioClip>();
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float checkDelay = 0.1f;

    private void Awake() {
        for (int i = 0; i < stepSoundsNames.Count; i++) {
            stepSounds.Add(stepSoundsNames[i], stepSoundsList[i]);
        }
    }

    public void CheckGround() {
        Ray ray = new Ray(transform.position, Vector3.down);
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, Vector3.down, out raycastHit)) {
            if (raycastHit.collider == null || raycastHit.collider.gameObject.GetComponent<MeshRenderer>() == null) {
                return;
            }

            if (raycastHit.collider.gameObject.layer == LayerMask.NameToLayer("Ground")) {
                AudioClip audioClip;
                stepSounds.TryGetValue(raycastHit.collider.gameObject.GetComponent<MeshRenderer>().material.name, out audioClip);
                if (audioClip == null) {
                    audioSource.clip = audioClip;   
                }
            }
        }
    }
}
