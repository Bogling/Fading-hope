using System.IO;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveLoadManager : MonoBehaviour
{
    private static string saveFilePath = Application.persistentDataPath + "/save.json";

    public static void Save() {
        string saveData = JsonUtility.ToJson(FindFirstObjectByType<GameManager>().GetComponent<GameManager>().GetData());
        File.WriteAllText(saveFilePath, saveData);
    }

    public static async void Load() {
        if (CheckFile()) {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            PlayerData playerData = new PlayerData();
            JsonUtility.FromJsonOverwrite(loadPlayerData, playerData);
            var asyncSceneLoad = SceneManager.LoadSceneAsync(playerData.GetScene(), LoadSceneMode.Single);
            while (!asyncSceneLoad.isDone) {
                await Task.Delay(5);
            }
            Debug.Log(FindFirstObjectByType(typeof(GameManager)));
            ((GameManager)FindFirstObjectByType(typeof(GameManager))).LoadData(playerData);

        }
    }

    public static PlayerData GetDirectData() {
        if (CheckFile()) {
            string loadPlayerData = File.ReadAllText(saveFilePath);
            PlayerData playerData = new PlayerData();
            JsonUtility.FromJsonOverwrite(loadPlayerData, playerData);
            return playerData;
        }
        return null;
    }

    public static bool CheckFile() {
        return File.Exists(saveFilePath);
    }

    public static void ResetData() {
        PlayerData playerData = new PlayerData();
        string saveData = JsonUtility.ToJson(playerData);
        File.WriteAllText(saveFilePath, saveData);
    }
}
