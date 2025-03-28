using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuManager : MonoBehaviour
{
    private static PauseMenuManager instance;
    [SerializeField] private DreamPlayerInputController dreamPlayerInputController;
    [SerializeField] private PlayerInputController playerInputController;
    [SerializeField] private OptionsMenu optionsMenu;

    private void Awake() {
        instance = this;
        gameObject.SetActive(false);
    }
    public static PauseMenuManager GetInstance() { return instance; }
    

    public void Pause() {
        if (dreamPlayerInputController != null) {
            dreamPlayerInputController.DisableInput();
        }
        if (playerInputController != null) {
            playerInputController.DisableInput();
            // Not implemented
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        gameObject.SetActive(true);
        Time.timeScale = 0.0f;
    }

    public void UnPause() {
        if (dreamPlayerInputController != null) {
            dreamPlayerInputController.EnableInput();
        }
        if (playerInputController != null) {
            playerInputController.EnableInput();
        }
        optionsMenu.gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        gameObject.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void OptionsButtonPressed() {
        gameObject.SetActive(false);
        optionsMenu.gameObject.SetActive(true);
    }

    public void ExitToTitleButtonPressed() {
        SaveLoadManager.Save();
        ToTitle(Color.black, 5f);
        
    }

    private async void ToTitle(Color faderColor, float fadeDuration) {
        UnPause();
        Fader.GetInstance().FadeOut(faderColor, fadeDuration);
        await Task.Delay((int)(fadeDuration * 1000));
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }

    public void QuitButtonPressed() {
        Application.Quit();
    }
}
