using UnityEngine;

public class MMContinueButton : MonoBehaviour
{
    private void Awake() {
        if (!SaveLoadManager.CheckFile()) {
            gameObject.SetActive(false);
        }
    }
}
