using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Color faderColor;
    [SerializeField] private bool isDream = true;
    [SerializeField] private float fadeDuration;
    [SerializeField] private string scene;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            StartCoroutine(ChangeScene());
        }
    }

    private IEnumerator ChangeScene() {
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        yield return new WaitForSeconds(fadeDuration);
        
        if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullDisable();
        }
        else if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullDisable();
        }
        SceneManager.LoadScene(scene);
        if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullEnable();
        }
        else if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullEnable();
        }
    }
}
