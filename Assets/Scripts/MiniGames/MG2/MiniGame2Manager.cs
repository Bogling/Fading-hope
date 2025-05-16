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
    private bool pendingCoin = false;
    private bool coinLocked = false;
    private bool isStarting = false;
    private bool dend = false;
    public static MiniGame2Manager GetInstance() {
        return instance;
    }

    private void Awake() {
        instance = this;
        gameManager = FindFirstObjectByType<GameManager>();
    }
   
    void Start()
    {
        gameObject.SetActive(false);
    }


    public void StartMiniGame() {
        talker.Lock();
        gameObject.SetActive(true);
        Talk(StartInkJSON[Random.Range(0, StartInkJSON.Length)]);
    }

    public void SelectOption(bool option) {
        if (option == rightOption) {
            Talk(OnRightInkJSON[Random.Range(0, OnRightInkJSON.Length)]);
        }
        else {
            Talk(OnWrongInkJSON[Random.Range(0, OnWrongInkJSON.Length)]);
        }
        pendingCoin = false;
    }

    private void EndMiniGame() {
        passedCount++;
        if (passedCount < PassCount) {
            dend = false;
            StartMiniGame();
        }
        else {
            Talk(EndInkJSON[Random.Range(0, EndInkJSON.Length)]);
        }
    }

    public void SetOption(bool option) {
        rightOption = option;
        Talk(QuestionInkJSON[Random.Range(0, QuestionInkJSON.Length)]);
    }

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
                        StartMiniGame();
                        break;
                    case 1:
                        QuitMiniGame();
                        break;
                }
                break;
            case 2:
                coinLocked = false;
                ChangeSprite("ithrow2");
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
        if (isStarting) {
            EndMiniGame();
            isStarting = false;
        }

        if (!coinFlipped && !pendingCoin && !coinLocked && !isStarting) {
            Lock();
            coin.Flip();
            pendingCoin = true;
            coinLocked = true;
        }
        return;
    }

    private void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }

    public void ChangeSprite(string spriteID) {
        if (spriteID == "ithrow4_n" || spriteID == "ithrow4_y") {
            isStarting = true;
        }
        if (spriteID == "ithrow4_n") {
            if (rightOption) {
                spriteID = "ithrow4_n2";
            }
            else {
                spriteID = "ithrow4_n1";
            }
        }
        else if (spriteID == "ithrow4_y") {
            if (rightOption) {
                spriteID = "ithrow4_y2";
            }
            else {
                spriteID = "ithrow4_y1";
            }
        }
        talker.ChangeSprite(spriteID);
        return;
    }


}
