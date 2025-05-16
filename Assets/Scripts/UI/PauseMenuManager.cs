using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private static PauseMenuManager instance;
    [SerializeField] private DreamPlayerInputController dreamPlayerInputController;
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private OptionsMenu optionsMenu;
    private bool isQuitting = false;

    private void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }
    public static PauseMenuManager GetInstance() { return instance; }
    

    public void Pause() {
        if (isQuitting) return;
        if (dreamPlayerInputController != null) {
            dreamPlayerInputController.DisableInput();
        }
        if (playerInputController != null) {
            playerInputController.DisableInput();
        }
        var s = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioSource in s) {
            if (audioSource.isPlaying) {
                audioSource.Pause();
            }
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPause() {
        if (isQuitting) return;
        if (dreamPlayerInputController != null) {
            dreamPlayerInputController.EnableInput();
        }
        if (playerInputController != null) {
            playerInputController.EnableInput();
        }
        optionsMenu.gameObject.SetActive(false);
        var s = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audioSource in s) {
            if (!audioSource.isPlaying && audioSource.time > 0.0f) {
                audioSource.UnPause();
            }
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OptionsButtonPressed() {
        if (isQuitting) return;
        gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void ExitToTitleButtonPressed() {
        if (isQuitting) return;
        SaveLoadManager.Save();
        ToTitle(Color.black, 5f);
        
    }

    private async void ToTitle(Color faderColor, float fadeDuration) {
        isQuitting = true;
        UnPause();
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        await Task.Delay((int)(fadeDuration * 1000));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        if (FindFirstObjectByType<DreamPlayerInputController>() != null) {
            FindFirstObjectByType<DreamPlayerInputController>().FullDisable();
        }
        else if (FindFirstObjectByType<PlayerInputController>() != null) {
            FindFirstObjectByType<PlayerInputController>().FullDisable();
        }
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButtonPressed() {
        Application.Quit();
    }
}
