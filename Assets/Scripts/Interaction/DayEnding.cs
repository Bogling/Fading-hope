using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DayEnding : MonoBehaviour
{
    private bool isLocked = true;
    [SerializeField] private string nextLevel;
    [SerializeField] private Color faderColor;
    [SerializeField] private float fadeDuration;
    [SerializeField] private GameObject notifier;
    private bool dend = false;

    private void Update() {
        if (isLocked) {
            return;
        }
        if (PlayerInputController.GetInstance().GetSubmitPressed()) {
            EndDay();
        }
    }

    private async void EndDay() {
        if (dend) return;
        dend = true;
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        notifier.SetActive(false);
        await Task.Delay((int)(fadeDuration * 1000));
        if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullDisable();
        }
        else if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullDisable();
        }
        SaveLoadManager.Save();
        SceneManager.LoadScene(nextLevel);
    }

    public void Unlock() {
        isLocked = false;
        notifier.SetActive(true);
    }

    public void ChangeNextLevel(string newLevel) {
        nextLevel = newLevel;
    }
}
