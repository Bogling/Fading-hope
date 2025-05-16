using TMPro;
using UnityEngine;

public class Ending : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textMeshPro1;
    [SerializeField] private TextMeshProUGUI textMeshPro2;
    void Start()
    {
        PlayerData playerData = SaveLoadManager.GetDirectData();
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (playerData.choice1 == false) {
            textMeshPro1.text = "Ending: C";
            textMeshPro2.text = "No more pain";
        }
        else if (playerData.choice1 == true
        && playerData.MaxMoodPoints == 4
        && playerData.beatDay5 == false) {
            textMeshPro1.text = "Ending: D";
            textMeshPro2.text = "Out of willpower";
        }
        else if (playerData.choice1 == true
        && playerData.beatDay5 == true
        && playerData.choice2 == false) {
            textMeshPro1.text = "Ending: B";
            textMeshPro2.text = "Fatal omen";
        }
        else if (playerData.choice1 == true
        && playerData.beatDay5 == true
        && playerData.choice2 == true) {
            textMeshPro1.text = "Ending: A";
            textMeshPro2.color = Color.green;
            textMeshPro2.text = "The new beginning";
        }
    }
}
