using System.Threading.Tasks;
using UnityEngine;

public class FallingBlock : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject == player) {
            Fall();
        }
    }

    private async void Fall() {
        await Task.Delay(1000);
        gameObject.SetActive(false);
    }
}
