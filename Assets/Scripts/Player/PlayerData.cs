using System.Collections.Generic;
using System.Linq;

public class PlayerData
{
    public string currentScene;
    public int currentCheckPoint;

    public bool hasLamp;
    public bool hasFlashlight;

    public bool hasHP;
    public bool hasLP;

    public int MaxMoodPoints = 5;
    public bool doubtedBio = false;
    
    //level variables
    public bool activatedCrystal1 = false;
    public bool activatedCrystal2 = false;
    public bool activatedCrystal3 = false;
    public int bigDoorStage = 0;
    public bool spokeToKyle1 = false;

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

    public int GetMaxMood() {
        return MaxMoodPoints;
    }

    public void SetMaxMood(int newMood) {
        MaxMoodPoints = newMood;
    }
}
