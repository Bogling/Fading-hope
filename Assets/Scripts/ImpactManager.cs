using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    private void OnParticleSystemStopped() {
        gameObject.SetActive(false);
    }
}
