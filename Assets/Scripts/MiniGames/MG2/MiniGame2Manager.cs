using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGame2Manager : MonoBehaviour, ITalkable
{
    [SerializeField] private testInteraction talker;
    [SerializeField] private Coin coin;
    private GameManager gameManager;

    [SerializeField] private TextAsset[] StartInkJSON;
    [SerializeField] private TextAsset[] EndInkJSON;
    [SerializeField] private TextAsset[] OnRightInkJSON;
    [SerializeField] private TextAsset[] OnWrongInkJSON;
    [SerializeField] private DialogueController dialogueController;

    [SerializeField] private int PassCount = 3;
    private int passedCount = 0;

    private bool isLocked = false;
    private int currentInk = 0;
    private static MiniGame2Manager instance;

    private bool rightOption;

    public static MiniGame2Manager GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        gameManager = FindFirstObjectByType<GameManager>();
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void StartMiniGame() {
        //a
        talker.Lock();
        gameObject.SetActive(true);
    }

    public void SelectOption(bool option) {
        if (option == rightOption) {
            Debug.Log("++++");
            EndMiniGame();
        }
        else {
            Debug.Log("----");
            EndMiniGame();
        }
    }

    private void EndMiniGame() {
        //b
        talker.Unlock();
        gameObject.SetActive(false);
    }

    public void SetOption(bool option) {
        rightOption = option;
    }

    // Talking Code

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
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        gameManager.DoubtedAnswer();
                        break;
                }
                break;
            case 1:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes1");
                        MiniGame1Manager.GetInstance().StartMiniGame();
                        isLocked = true;
                        //currentInk++;
                        break;
                    case 1:
                        Debug.Log("Answer is no1");
                        break;
                }
                break;
        }
    }

    public void Lock() {
        isLocked = true;
    }

    public void Unlock() {
        isLocked = false;
    }

    public void UponExit() {
        return;
    }
}
