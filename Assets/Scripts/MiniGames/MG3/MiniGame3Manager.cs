using System.Collections;
using UnityEditorInternal;
using UnityEngine;

public class MiniGame3Manager : MonoBehaviour, ITalkable
{
    [SerializeField] private DialogueController dialogueController;
    private MarkableSlot[][] markableSlots;
    [SerializeField] private MarkableSlot[] markableSlotsInline;

    [SerializeField] private TextAsset[] MGStartInkJSON;
    [SerializeField] private TextAsset[] MGEndInkJSON;
    [SerializeField] private TextAsset[] OnWinInkJSON;
    [SerializeField] private TextAsset[] OnLoseInkJSON;
    [SerializeField] private TextAsset[] OnDrawInkJSON;

    private bool isPlayersTurn = false;
    private int lastMoveXIndex = -1;
    private int lastMoveYIndex = -1;

    [SerializeField] private int PassCount = 3;
    private int passedCount = 0;

    private bool isLocked = false;
    private int currentInk = 0;
    private static MiniGame3Manager instance;
    [SerializeField] private Day3DialogueManager talker;
    private bool gameEnded = false;

    void Awake() {

        if (markableSlotsInline.Length != 9) {
            Debug.LogError("Markable slots amount does not match!");
        }

        markableSlots = new MarkableSlot[3][];
        for (int i = 0; i < 3; i++) {
            markableSlots[i] = new MarkableSlot[3];
            for (int j = 0; j < 3; j++) {
                markableSlots[i][j] = markableSlotsInline[i * 3 + j];
            }
        }

        instance = this;
    }

    void Start() {
        gameObject.SetActive(false);
    }

    public static MiniGame3Manager GetInstance() {
        return instance;
    }
    public bool GetPlayersTurn() {
        return isPlayersTurn;
    }

    public void MakeTurn(bool isPlayer, int moveXIndex, int moveYIndex) {
        CheckForCombination(isPlayer);
        lastMoveXIndex = moveXIndex;
        lastMoveYIndex = moveYIndex;
        if (isPlayer) {
            StartCoroutine(Think());
        }
    }

    private void CheckForCombination(bool isPlayer) {
        if (isPlayer) {
            if (markableSlots[0][0].GetMark() == 1 && markableSlots[0][1].GetMark() == 1 && markableSlots[0][2].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[1][0].GetMark() == 1 && markableSlots[1][1].GetMark() == 1 && markableSlots[1][2].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[2][0].GetMark() == 1 && markableSlots[2][1].GetMark() == 1 && markableSlots[2][2].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[0][0].GetMark() == 1 && markableSlots[1][0].GetMark() == 1 && markableSlots[2][0].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[0][1].GetMark() == 1 && markableSlots[1][1].GetMark() == 1 && markableSlots[2][1].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[0][2].GetMark() == 1 && markableSlots[1][2].GetMark() == 1 && markableSlots[2][2].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[0][0].GetMark() == 1 && markableSlots[1][1].GetMark() == 1 && markableSlots[2][2].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else if (markableSlots[0][2].GetMark() == 1 && markableSlots[1][1].GetMark() == 1 && markableSlots[2][0].GetMark() == 1) {
                CheckResultOfMiniGame(1);
            }
            else {
                foreach (var slots in markableSlots) {
                    foreach (var markableSlot in slots) {
                        if (!markableSlot.IsMarked()) {
                            return;
                        }
                    }
                }

                CheckResultOfMiniGame(0);
            }
        }
        else {
            if (markableSlots[0][0].GetMark() == -1 && markableSlots[0][1].GetMark() == -1 && markableSlots[0][2].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[1][0].GetMark() == -1 && markableSlots[1][1].GetMark() == -1 && markableSlots[1][2].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[2][0].GetMark() == -1 && markableSlots[2][1].GetMark() == -1 && markableSlots[2][2].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[0][0].GetMark() == -1 && markableSlots[1][0].GetMark() == -1 && markableSlots[2][0].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[0][1].GetMark() == -1 && markableSlots[1][1].GetMark() == -1 && markableSlots[2][1].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[0][2].GetMark() == -1 && markableSlots[1][2].GetMark() == -1 && markableSlots[2][2].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[0][0].GetMark() == -1 && markableSlots[1][1].GetMark() == -1 && markableSlots[2][2].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else if (markableSlots[0][2].GetMark() == -1 && markableSlots[1][1].GetMark() == -1 && markableSlots[2][0].GetMark() == -1) {
                CheckResultOfMiniGame(-1);
            }
            else {
                foreach (var slots in markableSlots) {
                    foreach (var markableSlot in slots) {
                        if (!markableSlot.IsMarked()) {
                            return;
                        }
                    }
                }

                CheckResultOfMiniGame(0);
            }
        }
    }

    private void CheckResultOfMiniGame(int winner) {
        switch (winner) {
            case -1:
                Talk(OnLoseInkJSON[Random.Range(0, OnLoseInkJSON.Length)]);
                gameEnded = true;
                break;
            case 0:
                Talk(OnDrawInkJSON[Random.Range(0, OnDrawInkJSON.Length)]);
                gameEnded = true;
                break;
            case 1:
                Talk(OnWinInkJSON[Random.Range(0, OnWinInkJSON.Length)]);
                gameEnded = true;
                break;
        }
    }
    private IEnumerator Think() {
        isPlayersTurn = false;
        yield return new WaitForSeconds(1f);
        if (lastMoveXIndex == -1 && lastMoveYIndex == -1) {
            markableSlots[Random.Range(0, 3)][Random.Range(0, 3)].Mark(false);
        }
        else {
            bool finished = false;
            bool hasEmptyAround = false;
                for (int i = Mathf.Clamp(lastMoveXIndex - 1, 0, 3); i < Mathf.Clamp(lastMoveXIndex + 1, 0, 3); i++) {
                    for (int j = Mathf.Clamp(lastMoveYIndex - 1, 0, 3); j < Mathf.Clamp(lastMoveYIndex + 1, 0, 3); j++) {
                        if (markableSlots[i][j].IsMarked() == false) {
                            hasEmptyAround = true;
                            break;
                        }
                    }
                    if (hasEmptyAround) {
                        break;
                    }
                }
            while (!finished) {
                if (hasEmptyAround) {
                    int xIndex = Mathf.Clamp(Random.Range(lastMoveXIndex - 1, lastMoveXIndex + 1), 0, 3);
                    int yIndex = Mathf.Clamp(Random.Range(lastMoveYIndex - 1, lastMoveYIndex + 1), 0, 3);

                    if (!markableSlots[xIndex][yIndex].IsMarked()) {
                        markableSlots[xIndex][yIndex].Mark(false);
                        finished = true;
                    }    
                    else {
                        yield return new WaitForSeconds(0.2f);
                    }
                }
                else {
                    int xIndex = Random.Range(0, 3);
                    int yIndex = Random.Range(0, 3);

                    if (!markableSlots[xIndex][yIndex].IsMarked()) {
                        markableSlots[xIndex][yIndex].Mark(false);
                        finished = true;
                    } 
                }
            }
        }

        isPlayersTurn = true;
    }

    public void Focus()
    {
        throw new System.NotImplementedException();
    }

    public void OperateChoice(int qID, int cID)
    {
        switch (qID) {
            case 0:
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

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void UponExit()
    {
        if (gameEnded) {
            if (passedCount < PassCount) {
                StartMiniGame();
                passedCount++;
            }
            else {
                Talk(MGEndInkJSON[Random.Range(0, MGEndInkJSON.Length)]);
                passedCount++;
            }
        }
    }

    public void StartMiniGame() {
        talker.Lock();
        gameObject.SetActive(true);
        ResetMiniGame();
        Talk(MGStartInkJSON[Random.Range(0, MGStartInkJSON.Length)]);
    }

    public void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }

    public void ResetMiniGame() {
        foreach (var slots in markableSlots) {
            foreach (var markableSlot in slots) {
                markableSlot.ResetMark();
            }
        }
        
        lastMoveXIndex = -1;
        lastMoveYIndex = -1;
        if (passedCount % 2 == 0) {
            isPlayersTurn = true;
        }
        else {
            isPlayersTurn = false;
            StartCoroutine(Think());
        }

        gameEnded = false;
    }
}
