using System.Collections;
using System.Collections.Generic;
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

    private void Update() {
        if (isLocked) {
            return;
        }
        if (PlayerInputController.GetInstance().GetSubmitPressed()) {
            EndDay();
        }
    }

    private async void EndDay() {
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        notifier.SetActive(false);
        await Task.Delay((int)(fadeDuration * 1000));
        SceneManager.LoadScene(nextLevel);
    }

    public void Unlock() {
        isLocked = false;
        notifier.SetActive(true);
    }
}
