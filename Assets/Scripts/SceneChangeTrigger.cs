using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChangeTrigger : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private Color faderColor;
    [SerializeField] private float fadeDuration;
    [SerializeField] private string scene;

    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            ChangeScene();
        }
    }

    private async void ChangeScene() {
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        await Task.Delay((int)(fadeDuration * 1000));
        SceneManager.LoadScene(scene);
    }
}
