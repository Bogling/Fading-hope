using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndingUI : MonoBehaviour
{
    public async void ToTitle() {
        Fader.GetInstance().FadeOut(Color.black, 1f);
        await Task.Delay(1000);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        SceneManager.LoadScene("MainMenu");
    }
}
