public class PlayerData
{
    public string currentScene;
    public int currentCheckPoint;

    public bool hasLamp;
    public bool hasFlashlight;

    public bool hasHP;
    public bool hasLP;

    public void SetScene(string scene) {
        currentScene = scene;
    }
    public void SetCheckPoint(int newCheckPoint) {
        currentCheckPoint = newCheckPoint;
    }

    public string GetScene() {
        return currentScene;
    }

    public int GetCheckPoint() {
        return currentCheckPoint;
    }
}
