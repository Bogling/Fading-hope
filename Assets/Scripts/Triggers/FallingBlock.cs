using System.Collections;
using System.Threading.Tasks;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private MeshRenderer meshRenderer;
    [SerializeField] private BoxCollider boxCollider;
    [SerializeField] private ParticleSystem destroyParticles;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject == player) {
            StartCoroutine(Fall());
        }
    }

    private IEnumerator Fall() {
        yield return new WaitForSeconds(2);
        boxCollider.enabled = false;
        meshRenderer.enabled = false;
        destroyParticles.Play();
        //gameObject.SetActive(false);
    }
}
