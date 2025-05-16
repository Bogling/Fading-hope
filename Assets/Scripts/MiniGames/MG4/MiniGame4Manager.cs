using System.Linq;
using UnityEngine;
using System.Collections;


public class MiniGame4Manager : MonoBehaviour, ITalkable, Interactable
{


    [Header("Ink Assets")]
    [SerializeField] private TextAsset[] StartMinigameInkJSON;
    [SerializeField] private TextAsset[] OnLoseMinigameInkJSON;
    [SerializeField] private TextAsset[] OnWinMinigameInkJSON;
    [SerializeField] private TextAsset[] AttackMinigameInkJSON;
    [SerializeField] private TextAsset[] EndMinigameInkJSON;

    [Header("Minigame Variables")]
    [SerializeField] private MG4AttackBoard attackBoard;
    [SerializeField] private MG4PlayerBoard playerBoard;
    [SerializeField] private int passCount;

    private int passedCount;
    private bool isPlayersTurn = true;
    private bool isSelecting = false;
    private SlotPosition currentStartPoint;
    private MiniGame4MarkableSlot[] selectedSlots;
    private bool isThinking = false;
    private DialogueController dialogueController;
    private Day3DialogueManager talker;
    private Animator animator;

    private string[] horizontalPositions = {"A", "B", "C", "D", "E", "F", "G", "H", "I", "J"};
    private string[] verticalPositions = {"1", "2", "3", "4", "5", "6", "7", "8", "9", "10"};
    private bool isHidden = false;
    private bool repeatEnemyTurn = false;
    private bool isEnding = false;

    private static MiniGame4Manager instance;

    void Awake() {
        instance = this;
        selectedSlots = new MiniGame4MarkableSlot[0];
        animator = GetComponent<Animator>();
    }

    void Start() {
        dialogueController = DialogueController.GetInstance();
        talker = FindFirstObjectByType<Day3DialogueManager>();
        gameObject.SetActive(false);
    }

    public static MiniGame4Manager GetInstance() { 
        return instance; 
    }
    public bool IsPlayersTurn() { return isPlayersTurn; }
    public bool IsHidden() { return isHidden; }

    public void StartMiniGame() {
        talker.Lock();
        gameObject.SetActive(true);
        ResetMiniGame();
        Talk(StartMinigameInkJSON[Random.Range(0, StartMinigameInkJSON.Length)]);
    }

    public void EndMiniGame(bool isPlayerWin) {
        isEnding = true;
        if (isPlayerWin) {
            Talk(OnWinMinigameInkJSON[Random.Range(0, OnWinMinigameInkJSON.Length)]);
        }
        else{
            Talk(OnLoseMinigameInkJSON[Random.Range(0, OnLoseMinigameInkJSON.Length)]);
        }

        StartCoroutine(EndHandler());
    }

    private IEnumerator EndHandler() {
        while (isEnding) {
            yield return new WaitForSeconds(0.1f);
        }
        passedCount++;
        if (passedCount < passCount) {
            StartMiniGame();
        }
        else {
            Talk(EndMinigameInkJSON[Random.Range(0, EndMinigameInkJSON.Length)]);
        }
        isEnding = false;
    }

    public void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }

    public void ResetMiniGame() {
        playerBoard.Reset();
        attackBoard.Reset();
        playerBoard.StartPlacingStage();
    }

    public void MakeTurn(bool isPlayer, SlotPosition slot) {
        if (isEnding) return;
        if (isPlayer) {
            attackBoard.GetSlot(slot).Mark(true, false);
            if (attackBoard.ManageShipDamage(slot)) {
                if (attackBoard.AreAnyShipLeft()) {
                    return;
                }
                else {
                    EndMiniGame(true);
                    return;
                }
            }
            if (attackBoard.AreAnyShipLeft()) {
                StartCoroutine(Think());           
            }
            else {
                EndMiniGame(true);
                return;
            } 
        }
        else {
            TypeTurn(AttackMinigameInkJSON[Random.Range(0, AttackMinigameInkJSON.Length)], slot);
            playerBoard.GetSlot(slot).Mark(false, false);
            if (playerBoard.ManageShipDamage(slot)) {
                if (playerBoard.AreAnyShipLeft()) {
                    repeatEnemyTurn = true;
                }
                else {
                    EndMiniGame(false);
                    return;
                }
            }
            if (!playerBoard.AreAnyShipLeft()) {
                EndMiniGame(false);
                return;
            }
        }
    }

    public IEnumerator Think() {
        if (isThinking || isEnding) {
            yield break;
        }
        isThinking = true;
        isPlayersTurn = false;
        yield return new WaitForSeconds(Random.Range(1, 3));
        playerBoard.Attack();
        isPlayersTurn = true;
        isThinking = false;
    }

    public bool IsSelecting() {
        return isSelecting;
    }

    public bool CanSelect() {
        return playerBoard.IsInPlacingStage() && !isSelecting;
    }

    public void SelectForShipPlacement(SlotPosition slotPosition) {
        isSelecting = true;
        currentStartPoint = slotPosition;
        TraceLine(currentStartPoint);
    }

    public void TraceLine(SlotPosition destinationSlotPosition) {
        switch (GetDirection(destinationSlotPosition)) {
            case -1:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];
                break;
            case 0:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];

                for (int i = (int)Mathf.Min(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i <= (int)Mathf.Max(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i++) {
                    if (playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).IsMarked() || playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).IsLocked()) {
                        return;
                    }
                }

                for (int i = (int)Mathf.Min(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i <= (int)Mathf.Max(currentStartPoint.XIndex, destinationSlotPosition.XIndex); i++) {
                    selectedSlots = selectedSlots.Append(playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex))).ToArray();
                    playerBoard.GetSlot(new SlotPosition(i, currentStartPoint.YIndex)).Select();
                }
                break;
            case 1:
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }
                selectedSlots = new MiniGame4MarkableSlot[0];

                for (int i = (int)Mathf.Min(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i <= (int)Mathf.Max(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i++) {
                    if (playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).IsMarked() || playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).IsLocked()) {
                        return;
                    }
                }

                for (int i = (int)Mathf.Min(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i <= (int)Mathf.Max(currentStartPoint.YIndex, destinationSlotPosition.YIndex); i++) {
                    selectedSlots = selectedSlots.Append(playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i))).ToArray();
                    playerBoard.GetSlot(new SlotPosition(currentStartPoint.XIndex, i)).Select();
                }
                break;
        }
    }

    public void SubmitLine() {
        isSelecting = false;
        if (selectedSlots.Length > 0) {
            if (playerBoard.AddShip(selectedSlots)) {
                selectedSlots = new MiniGame4MarkableSlot[0];
            }
            else {
                foreach (MiniGame4MarkableSlot slot in selectedSlots) {
                    slot.UnSelect();
                }

                selectedSlots = new MiniGame4MarkableSlot[0];
            }

            if (playerBoard.CheckIfAllShipsPlaced()) {
                playerBoard.EndPlacingStage();
                attackBoard.GenerateShips();

                if (passedCount % 2 == 0) {
                    isPlayersTurn = true;
                }
                else {
                    StartCoroutine(Think());
                }
            }
        }
    }

    private int GetDirection(SlotPosition destinationSlotPosition) {
        if (currentStartPoint.XIndex == destinationSlotPosition.XIndex) {
            return 1;
        }
        else if (currentStartPoint.YIndex == destinationSlotPosition.YIndex) {
            return 0;
        }
        else {
            return -1;
        }
    }

    public void Focus()
    {
        FindFirstObjectByType<PlayerCam>().LookAtPosition(transform, 2);
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    private void TypeTurn(TextAsset inkJSON, SlotPosition slotPosition) {
        string text = horizontalPositions[^(slotPosition.XIndex + 1)] + verticalPositions[^(slotPosition.YIndex + 1)];
        dialogueController.EnterDialogue(inkJSON, this, "markPoint", text);
    }

    public void OperateChoice(int qID, int cID)
    {
        switch (qID) {
            case 0:
                switch (cID) {
                    case 0:
                        isEnding = false;
                        StartMiniGame();
                        break;
                    case 1:
                        QuitMiniGame();
                        break;
                }
                break;
        }
    }

    public void UponExit()
    {
        if (isEnding) {
            isEnding = false;
            return;
        }
        if (repeatEnemyTurn) {
            StartCoroutine(Think());
            repeatEnemyTurn = false;
        }
    }

    public bool IsCurrentlyInteractable()
    {
        return true;
    }

    public void Interact()
    {
        if (isHidden) {
            animator.SetTrigger("Show");
            isHidden = false;
        }
        else {
            animator.SetTrigger("Hide");
            isHidden = true;
        }
    }

    public void OnHover()
    {
        return;
    }

    public void OnHoverStop()
    {
        return;
    }

    public void InteractionCanceled()
    {
        return;
    }

    public void ChangeSprite(string spriteID) {
        talker.ChangeSprite(spriteID);
    }
}