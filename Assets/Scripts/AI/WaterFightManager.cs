using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterFightManager : MonoBehaviour, Interactable
{
    [SerializeField] private Door[] doors;
    [SerializeField] private Door exitDoor;
    [SerializeField] private float waveDelay;
    [SerializeField] private FearAI[] fears;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip fClip;
    [SerializeField] private AudioClip defClip;
    private List<FearAI> currentEnemies = new List<FearAI>();
    private int currentStage = 0;

    private void Start() {
        if (doors.Length != 6) {
            Debug.LogError("Number of doors should be 6");
        }


    }

    public IEnumerator StartWave() {
        if (currentStage == 0) {
            exitDoor.Close();
            audioSource.clip = fClip;
            audioSource.Play();
        }

        foreach (var enemy in currentEnemies) {
            if (!enemy.IsDead()) {
                yield break;
            }
        }

        yield return new WaitForSeconds(waveDelay);
        currentStage++;

        if (currentStage >= 7) {
            exitDoor.Open();
            audioSource.clip = defClip; 
            audioSource.Play();
            yield break;
        }

        currentEnemies.Clear();
        currentEnemies.Add(fears[currentStage - 1]);
        fears[currentStage - 1].Wake(true);
        doors[currentStage - 1].Open();
        fears[currentStage - 1].SetDestination(gameObject);
    }

    public bool IsCurrentlyInteractable()
    {
        return false;
    }

    public void Interact()
    {
        StartCoroutine(StartWave());
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void InteractionCanceled()
    {
        return;
    }
}
