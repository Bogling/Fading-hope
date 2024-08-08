using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class AscendTrigger : MonoBehaviour, ITalkable
{
    [SerializeField] private GameObject player;
    private void OnTriggerEnter(Collider other) {
        if (other.gameObject == player) {
            Talk(inkJSON[currentInk]);
        }
    }

    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;

    private int currentInk = 0;

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes");
                        End();
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        break;
                }
            break;
        }
    }

    private async void End() {
        Fader.GetInstance().FadeOut(Color.white, 5f);
        await Task.Delay((int)(5f * 1000));
        Application.Quit();
    }
}
