using System.Collections;
using UnityEngine;

public class GreenCrystal : MonoBehaviour, IDamageable
{
    private bool isActivated = false;
    [SerializeField] private GameObject teleportPoint;
    [SerializeField] private Fader fader;
    [SerializeField] private GameObject player;
    [SerializeField] private Color fadeColor;
    [SerializeField] private BigGreenCrystal bigGreenCrystal;
    [SerializeField] private GameObject DMesh;
    [SerializeField] private GameObject AMesh;
    

    private void Start() {
        DMesh.SetActive(true);
        AMesh.SetActive(false);
    }

    public void Activate() {
        isActivated = true;
        AMesh.SetActive(true);
        DMesh.SetActive(false);
        bigGreenCrystal.ManageLocks();
        StartCoroutine(Teleport());
    }

    public IEnumerator Teleport() {
        fader.FadeOut(fadeColor, 1f);
        yield return new WaitForSeconds(1.1f);
        player.transform.position = teleportPoint.transform.position;
        fader.FadeIn(fadeColor, 1f);
        yield return new WaitForSeconds(1f);
    }

    public bool IsActivated() {
        return isActivated;
    }

    public void DealDamage(float damage)
    {
        if (!isActivated) {
            Activate();
        }
    }
}
