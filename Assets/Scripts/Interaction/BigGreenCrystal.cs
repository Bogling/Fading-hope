using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class BigGreenCrystal : MonoBehaviour, IDamageable
{
    [SerializeField] private GreenCrystal[] crystals;
    [SerializeField] private Door[] doorsToLock;
    [SerializeField] private LightReceiver[] receiversToLock;
    [SerializeField] private Fader fader;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject[] pointLights;
    [SerializeField] private GameObject DMesh;
    [SerializeField] private GameObject AMesh;

    private bool isActivated;

    [SerializeField] private GameObject teleportPoint;

    private void Start() {
        DMesh.SetActive(true);
        AMesh.SetActive(false);

        foreach (var pointLight in pointLights) {
            pointLight.SetActive(false);
        }
    }

    public bool IsCurrentlyInteractable()
    {
        foreach (var crystal in crystals) {
            if (!crystal.IsActivated()) {
                return false;
            }
        }

        return true;
    }

    public void ManageLocks() {
        for (int i = 0; i < crystals.Length; i++) {
            if (crystals[i].IsActivated()) {
                if (doorsToLock[i].IsOpened()) {
                    doorsToLock[i].Close();
                }
                doorsToLock[i].Lock();
                receiversToLock[i].Lock();
                pointLights[i].SetActive(true);
            }
        }
    }

    public bool[] GetCrystalStates() {
        bool[] states = new bool[crystals.Length];
        for (int i = 0; i < crystals.Length; i++) {
            states[i] = crystals[i].IsActivated();
        }
        return states;
    }

    public void SetCrystalStates(bool[] states) {
        for (int i = 0; i < crystals.Length; i++) {
            if (states[i]) {
                crystals[i].Activate();
            }
        }
    }

    //public IEnumerator Teleport() {
    //
    //}

    public void DealDamage(float damage)
    {
        throw new System.NotImplementedException();
    }
}