using UnityEngine;

public class ImpactManager : MonoBehaviour
{
    private void OnParticleSystemStopped() {
        Destroy(gameObject);
    }
}
