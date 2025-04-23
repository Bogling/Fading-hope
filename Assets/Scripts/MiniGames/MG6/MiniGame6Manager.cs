using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class MiniGame6Manager : MonoBehaviour, ITalkable
{
    [Header("Spawn parameters")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private float spawnGap;

    [Header("Card Prefabs")]
    [SerializeField] private MG6Card cardHPrefab;
    [SerializeField] private MG6Card cardDPrefab;
    [SerializeField] private MG6Card cardCPrefab;
    [SerializeField] private MG6Card cardSPrefab;
    [Header("Decks")]
    [SerializeField] private MG6Deck playerDeck;
    [SerializeField] private MG6Deck enemyDeck;

    [Header("Minigame Settings")]
    [SerializeField] private int cardsCount;
    [SerializeField] private float assignDelay;
    [SerializeField] private int maxRethinkTimes;
    [SerializeField] private int passCount;

    [Header("Ink JSON files")]
    [SerializeField] private TextAsset[] startInkJSON;
    [SerializeField] private TextAsset[] endInkJSON;
    [SerializeField] private TextAsset[] playerLuckInkJSON;
    [SerializeField] private TextAsset[] playerUnluckInkJSON;
    [SerializeField] private TextAsset[] playerWrongSelectionInkJSON;
    [SerializeField] private TextAsset[] playerNormalSelectionInkJSON;
    [SerializeField] private TextAsset[] enemyWrongSelectionInkJSON;
    [SerializeField] private TextAsset[] enemyNormalSelectionInkJSON;
    [SerializeField] private TextAsset[] playerWinInkJSON;
    [SerializeField] private TextAsset[] playerLoseInkJSON;

    private Dictionary<int, List<MG6Card>> allCards = new Dictionary<int, List<MG6Card>>();
    private List<MG6Card> cardsH = new List<MG6Card>();
    private List<MG6Card> cardsD = new List<MG6Card>();
    private List<MG6Card> cardsC = new List<MG6Card>();
    private List<MG6Card> cardsS = new List<MG6Card>();


    private bool isSelectionStage = false;
    private bool isPlayerTurn = false;
    private int cardRandomStartDivider = 1;
    private const int MAXCARDSCOUNT = 49;
    private bool nextPlayerTurn = false;
    private int passedCount = 0;
    Day4DialogueManager talker;
    DialogueController dialogueController;
    private static MiniGame6Manager instance;
    
    private void Awake() {
        instance = this;

        if (cardsCount > MAXCARDSCOUNT) {
            Debug.LogError("Number of cards is too large");
        }

        if ((MAXCARDSCOUNT - cardsCount) % 4 != 0) {
            Debug.LogError("Number of cards is invalid");
        }

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueController = DialogueController.GetInstance();
        cardRandomStartDivider = (MAXCARDSCOUNT - cardsCount) / 4;
        cardsH = new List<MG6Card>((cardsCount - 1) / 4);
        cardsD = new List<MG6Card>((cardsCount - 1) / 4);
        cardsC = new List<MG6Card>((cardsCount - 1) / 4);
        cardsS = new List<MG6Card>(((cardsCount - 1) / 4) + 1);
        talker = FindFirstObjectByType<Day4DialogueManager>();
    }

    public static MiniGame6Manager GetInstance() { return instance; }

    public bool IsSelectionStage() { return isSelectionStage; }

    private void GenerateCards() {
        for (int i = 0; i < cardsCount; i++) {
            int type;
            while (true) {
                type = Random.Range(0, 4);

                if (type == 0 && cardsH.Count < (cardsCount - 1) / 4) {
                    break;
                }
                else if (type == 1 && cardsD.Count < (cardsCount - 1) / 4) {
                    break;
                }
                else if (type == 2 && cardsC.Count < (cardsCount - 1) / 4) {
                    break;
                }
                else if (type == 3 && cardsS.Count < ((cardsCount - 1) / 4) + 1) {
                    break;
                }
            }

            switch (type) {
                case 0:
                    MG6Card cardH = Instantiate(cardHPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(cardRandomStartDivider, 13);
                        if (index == 10) { continue; }
                        if (CheckForCardInType(cardsH, index)) {
                            continue;
                        }

                        cardH.SetCard(type, index, i);
                        break;
                    }
                    cardsH.Add(cardH);
                    allCards.Add(i, cardsH);
                    break;
                case 1:
                    MG6Card cardD = Instantiate(cardDPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(cardRandomStartDivider, 13);
                        if (index == 10) { continue; }
                        if (CheckForCardInType(cardsD, index)) {
                            continue;
                        }

                        cardD.SetCard(type, index, i);
                        break;
                    }
                    cardsD.Add(cardD);
                    allCards.Add(i, cardsD);
                    break;
                case 2:
                    MG6Card cardC = Instantiate(cardCPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(cardRandomStartDivider, 13);
                        if (index == 10) { continue; }
                        if (CheckForCardInType(cardsC, index)) {
                            continue;
                        }

                        cardC.SetCard(type, index, i);
                        break;
                    }
                    cardsC.Add(cardC);
                    allCards.Add(i, cardsC);
                    break;
                case 3:
                    MG6Card cardS = Instantiate(cardSPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(cardRandomStartDivider, 13);
                        if (CheckForCardInType(cardsS, index)) {
                            continue;
                        }

                        cardS.SetCard(type, index, i);
                        break;
                    }
                    cardsS.Add(cardS);
                    allCards.Add(i, cardsS);
                    break;
            }
        }
    }

    private bool CheckForCardInType(List<MG6Card> list, int index) {
        foreach (MG6Card c in list) {
            if (index == c.GetIndex()) {
                return true;
            }
        }

        return false;
    }

    private IEnumerator AssignDecks() {
        bool playerT = true;

        while (allCards.Count > 0) {
            if (playerT) {
                playerDeck.AddCard(allCards[allCards.Count - 1].Find(x => x.GetGlobalIndex() == allCards.Count - 1));
                allCards.Remove(allCards.Count - 1);
                yield return new WaitForSeconds(assignDelay);
            }
            else {
                enemyDeck.AddCard(allCards[allCards.Count - 1].Find(x => x.GetGlobalIndex() == allCards.Count - 1));
                allCards.Remove(allCards.Count - 1);
                yield return new WaitForSeconds(assignDelay);
            }

            playerT = !playerT;
        }      
        
        if (playerDeck.HasQueen()) {
            if (Random.Range(0, 2) == 1) {
                Talk(playerUnluckInkJSON[Random.Range(0, playerUnluckInkJSON.Length - 1)]);
            }
        }
        else {
            if (Random.Range(0, 2) == 1) {
                Talk(playerLuckInkJSON[Random.Range(0, playerLuckInkJSON.Length - 1)]);
            }
        }

        EnterSelectionStage();
    }

    public void EnterSelectionStage() {
        isSelectionStage = true;
        if (playerDeck.ContainsPairs()) {
            StartCoroutine(playerDeck.ShowDeck());
        }
        
        StartCoroutine(enemyDeck.FindPairs());
    }

    public void FinishSelectionStage(bool isPlayer) {
        if (isPlayer) {
            StartCoroutine(playerDeck.HideDeck());
        }
        if (playerDeck.ContainsPairs() || enemyDeck.ContainsPairs() || !isSelectionStage) {
            return;
        }
        
        isSelectionStage = false;

        if (CheckForGameOver() == 1) {
            EndMiniGame(true);
            return;
        }
        else if (CheckForGameOver() == -1) {
            EndMiniGame(false);
            return;
        }

        if (nextPlayerTurn) {
            isPlayerTurn = true;
            playerDeck.Lock();
            enemyDeck.Unlock();
            nextPlayerTurn = false;
        }
        else {
            isPlayerTurn = false;
            playerDeck.Lock();
            enemyDeck.Lock();
            StartCoroutine(Think());
            nextPlayerTurn = true;
        }
    }

    public IEnumerator Think() {
        int rethinkTimes = 0;
        while (true) {
            yield return new WaitForSeconds(Random.Range(1, 3));
            playerDeck.PickRandomCard();
            yield return new WaitForSeconds(Random.Range(1, 3));
            if (Random.Range(0, 2) == 1 && rethinkTimes < maxRethinkTimes) {
                rethinkTimes++;
                continue;
            }

            playerDeck.SubmitPick();
            break;
        }
    }

    public void CardPicked(MG6Card card, MG6Deck deck) {
        if (deck == playerDeck) {
            playerDeck.RemoveCard(card);
            enemyDeck.AddCard(card);

            if (card.GetIndex() == 10) {
                Talk(enemyWrongSelectionInkJSON[Random.Range(0, enemyWrongSelectionInkJSON.Length - 1)]);
            }
            else if (playerDeck.HasQueen()) {
                Talk(enemyNormalSelectionInkJSON[Random.Range(0, enemyNormalSelectionInkJSON.Length - 1)]);
            }

            enemyDeck.ShuffleDeck();
        }
        else {
            enemyDeck.RemoveCard(card);
            playerDeck.AddCard(card);

            if (card.GetIndex() == 10) {
                Talk(playerWrongSelectionInkJSON[Random.Range(0, playerWrongSelectionInkJSON.Length - 1)]);
            }
            else if (enemyDeck.HasQueen()) {
                Talk(playerNormalSelectionInkJSON[Random.Range(0, playerNormalSelectionInkJSON.Length - 1)]);
            }
        }
        
        EnterSelectionStage();
    }

    private int CheckForGameOver() {
        if (playerDeck.IsDeckEmpty() && enemyDeck.CheckLosingCondition()) {
            return 1;
        }
        else if (enemyDeck.IsDeckEmpty() && playerDeck.CheckLosingCondition()) {
            return -1;
        }
        else {
            return 0;
        }
    }

    public void StartMiniGame() {
        talker.Lock();
        gameObject.SetActive(true);

        Talk(startInkJSON[Random.Range(0, startInkJSON.Length - 1)]);

        cardsH = new List<MG6Card>((cardsCount - 1) / 4);
        cardsD = new List<MG6Card>((cardsCount - 1) / 4);
        cardsC = new List<MG6Card>((cardsCount - 1) / 4);
        cardsS = new List<MG6Card>(((cardsCount - 1) / 4) + 1);
        allCards.Clear();

        playerDeck.Reset();
        enemyDeck.Reset();
        
        if (passedCount % 2 == 0) {
            nextPlayerTurn = true;
        }
        else {
            nextPlayerTurn = false;
        }

        playerDeck.HideDeck();
        GenerateCards();
        StartCoroutine(AssignDecks());
        if (playerDeck.HasQueen()) {
            Talk(playerUnluckInkJSON[Random.Range(0, playerUnluckInkJSON.Length - 1)]);
        }
        else {
            Talk(playerLuckInkJSON[Random.Range(0, playerLuckInkJSON.Length - 1)]);
        }
    }

    private void EndMiniGame(bool playerWins) {
        passedCount++;
        if (playerWins) {
            Talk(playerWinInkJSON[Random.Range(0, playerWinInkJSON.Length - 1)]);
        }
        else {
            Talk(playerLoseInkJSON[Random.Range(0, playerLoseInkJSON.Length - 1)]);
        }

        if (passedCount < passCount) {
            StartMiniGame();
        }
        else {
            Talk(endInkJSON[Random.Range(0, endInkJSON.Length - 1)]);            
        }
    }

    private void QuitMiniGame() {
        talker.Unlock();
        gameObject.SetActive(false);
        talker.Interact();
    }

    public void Focus()
    {
        throw new System.NotImplementedException();
    }

    public void Talk(TextAsset inkJSON)
    {
        dialogueController.EnterDialogue(inkJSON, this);
    }

    public void OperateChoice(int qID, int cID)
    {
        switch (qID)
        {
            case 0:
                switch (cID)
                {
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

    public void UponExit()
    {
        return;
    }

    public void ChangeSprite(string spriteID) {
        talker.ChangeSprite(spriteID);
    }
}
