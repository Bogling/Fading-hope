using UnityEngine;

public class MiniGame2Manager : MonoBehaviour, ITalkable
{
    [SerializeField] private Day2DialogueManager talker;
    [SerializeField] private Coin coin;
    private GameManager gameManager;

    [SerializeField] private TextAsset[] StartInkJSON;
    [SerializeField] private TextAsset[] EndInkJSON;

    [SerializeField] private TextAsset[] QuestionInkJSON;
    [SerializeField] private TextAsset[] OnRightInkJSON;
    [SerializeField] private TextAsset[] OnWrongInkJSON;
    [SerializeField] private DialogueController dialogueController;

    [SerializeField] private int PassCount = 3;
    private int passedCount = 0;

    private bool isLocked = false;
    private int currentInk = 0;

    private bool coinFlipped = false;
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
        Talk(StartInkJSON[Random.Range(0, StartInkJSON.Length)]);
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
        if (passedCount < PassCount) {
            StartMiniGame();
            passedCount++;
        }
        else {
            Talk(EndInkJSON[Random.Range(0, EndInkJSON.Length)]);
            passedCount++;
        }
    }

    public void SetOption(bool option) {
        rightOption = option;
        talker.ChangeSprite(1);
        Talk(QuestionInkJSON[Random.Range(0, QuestionInkJSON.Length)]);
    }

    // Talking Code

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void Focus() {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 0.5f);
    }

    public void OperateChoice(int qID, int cID) {
        switch (qID) {
            case 0:
                SelectOption(System.Convert.ToBoolean(cID));
                break;
            case 1:
                switch (cID) {
                    case 0:
                        Debug.Log("Answer is yes1");
                        StartMiniGame();
                        break;
                    case 1:
                        Debug.Log("Answer is no1");
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
        if (!coinFlipped) {
            Lock();
            talker.ChangeSprite(0);
            coin.Flip();
        }
        return;
    }

    private void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }
}
