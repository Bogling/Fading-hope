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
    private SpriteRenderer spriteRenderer;
    private bool rightOption;
    private int passedCount = 0;

    private bool isLocked = false;
    private int currentImg = 0;

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
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = null;
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
        if (option) {
            if (option == rightOption) {
                currentImg = 1;
            }
            else {
                currentImg = 2;
            }
        }
        else {
            if (option == rightOption) {
                currentImg = 3;
            }
            else {
                currentImg = 4;
            }
        }
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

    public void ChangeSprite(string spriteID) {
        talker.ChangeSprite(spriteID);
        switch (currentImg) {
            case 0:
                spriteRenderer.sprite = null;
                break;
            case 1:
                spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("ihide3_l2");
                currentImg = 0;
                break;
            case 2:
                spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("ihide3_l1");
                currentImg = 0;
                break;
            case 3:
                spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("ihide3_r2");
                currentImg = 0;
                break;
            case 4:
                spriteRenderer.sprite = MiraSpritesData.GetInstance().GetSprite("ihide3_r1");
                currentImg = 0;
                break;
        }
    }
}
