using UnityEngine;

public class LightBridge : MonoBehaviour
{
    [SerializeField] private GameObject bridge;

    private void Awake() {
        bridge.SetActive(false);
    }

    private void Update() {
        if (GetComponent<Lever>().isActivated) {
            bridge.SetActive(true);
        }
        else return;
    }
}
