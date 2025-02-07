using UnityEngine;

public class ScreenShakeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            FindFirstObjectByType<DreamPlayerCam>().ShakeScreen();
            gameObject.SetActive(false);
        }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
