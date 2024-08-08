using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private string NGScene;
    [SerializeField] private float fadeTime;
    [SerializeField] private OptionsMenu optionsMenu;
    public async void NewGame() {
        Fader.GetInstance().FadeOut(Color.black, fadeTime);
        await Task.Delay((int)(fadeTime * 1000));
        SceneManager.LoadScene(NGScene);
    }

    public async void Continue() {
        if (!SaveLoadManager.CheckFile()) return;
        Fader.GetInstance().FadeOut(Color.black, fadeTime);
        await Task.Delay((int)(fadeTime * 1000));
        SaveLoadManager.Load();
    }

    public void Options() {
        optionsMenu.gameObject.SetActive(true);
    }

    public async void Quit() {
        Fader.GetInstance().FadeOut(Color.black, fadeTime);
        await Task.Delay((int)(fadeTime * 1000));
        Application.Quit();
    }
}
