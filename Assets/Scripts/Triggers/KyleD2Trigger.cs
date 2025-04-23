using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KyleD2Trigger : MonoBehaviour, ITalkable
{
    [SerializeField] private GameObject player;
    [SerializeField] private Animator kyleAnimator;
    [SerializeField] private Transform focusPosition;
    [SerializeField] private float focusSpeed;
    [SerializeField] private TextAsset[] inkJSON;
    [SerializeField] private DreamDialogueController dialogueController;
    private int currentInk;
    private bool choice;
    void OnTriggerEnter(Collider other)
    {
        if (FindFirstObjectByType<GameManager>().GetKyleSp1Par()) {
            currentInk = 0;
        }
        else {
            currentInk = 1;
        }
        Talk(inkJSON[currentInk]);
        
    }

    public void Focus()
    {
        FindFirstObjectByType<DreamPlayerCam>().LookAtPosition(focusPosition, focusSpeed);
    }

    public void OperateChoice(int qID, int cID)
    {
        switch (qID) {
            case 0:
                kyleAnimator.SetTrigger("D2T1");
                break;
            case 1:
                kyleAnimator.SetTrigger("D2T2");
                break;
            case 2:
                kyleAnimator.SetTrigger("D2T3");
                break;
            case 3:
                kyleAnimator.SetTrigger("D2T4");
                break;
            case 4:
                kyleAnimator.SetTrigger("D2T5");
                break;
            case 5:
                kyleAnimator.SetTrigger("D2T6");
                break;
            case 6:
                switch (cID) {
                    case 0:
                        choice = false;
                        kyleAnimator.SetTrigger("D2T");
                        break;
                    case 1:
                        choice = true;
                        break;
                }
                break;
        }
    }

    public void Talk(TextAsset inkJSON)
    {
        Focus();
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void UponExit()
    {
        if (choice == false) {
            StartCoroutine(ChangeScene("Ending"));
        }
        else {
            StartCoroutine(ChangeScene("Day5"));
        }
    }    

    public void ChangeSprite(string spriteID) {
        return;
    }

    private IEnumerator ChangeScene(string nextLevel) {
        Fader.GetInstance().FadeOut(Color.black, 1f);
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(nextLevel);
    }
}
