using UnityEngine;

public class MiniGame1Manager : MonoBehaviour, ITalkable
{
    [SerializeField] private Day2DialogueManager talker;

    [SerializeField] private TextAsset[] StartInkJSON;
    [SerializeField] private TextAsset[] EndInkJSON;
    [SerializeField] private TextAsset[] OnRightInkJSON;
    [SerializeField] private TextAsset[] OnWrongInkJSON;
    [SerializeField] private DialogueController dialogueController;
    [SerializeField] private int PassCount = 3;
    private static MiniGame1Manager instance;
    private bool rightOption;
    private int passedCount = 0;

    private bool isLocked = false;

    public static MiniGame1Manager GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void StartMiniGame() {
        //a
        talker.Lock();
        //Interrupt();
        Talk(StartInkJSON[Random.Range(0, StartInkJSON.Length)]);
        GenerateOption();
        gameObject.SetActive(true);
    }

    public void SelectOption(bool option) {
        if (option == rightOption) {
            Debug.Log("++++");
            Talk(OnRightInkJSON[Random.Range(0, OnRightInkJSON.Length)]);
            EndMiniGame();
        }
        else {
            Debug.Log("----");
            Talk(OnWrongInkJSON[Random.Range(0, OnWrongInkJSON.Length)]);
            EndMiniGame();
        }
    }

    private void EndMiniGame() {
        //b
        passedCount++;
        if (passedCount < PassCount) {
            StartMiniGame();
        }
        else {
            Talk(EndInkJSON[Random.Range(0, EndInkJSON.Length)]);
        }
        //talker.Unlock();
        //gameObject.SetActive(false);
    }

    private void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }

    private void GenerateOption() {
        rightOption = Random.Range(0, 2) == 1;
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 2);
    }

    private void Interrupt() {
        dialogueController.forceEndDialogue();
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes");
                        StartMiniGame();
                        break;
                    case 1:
                        Debug.Log("Answer is no");
                        QuitMiniGame();
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
