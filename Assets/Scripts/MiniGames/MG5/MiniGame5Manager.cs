using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class MiniGame5Manager : MonoBehaviour, ITalkable
{
    private Dictionary<int, List<MG5Card>> allCards = new Dictionary<int, List<MG5Card>>();
    private List<MG5Card> cardsH = new List<MG5Card>(9);
    private List<MG5Card> cardsD = new List<MG5Card>(9);
    private List<MG5Card> cardsC = new List<MG5Card>(9);
    private List<MG5Card> cardsS = new List<MG5Card>(9);
    
    [SerializeField] private int PassCount;
    private int passedCount = 0;

    [Header("Spawn Params")]
    [SerializeField] private GameObject spawnPoint;
    [SerializeField] private float spawnGap;
    [SerializeField] private float spawnDelay;

    [Header("Grid Params")]
    [SerializeField] private GameObject destinationPoint;
    [SerializeField] private int cardRows;
    [SerializeField] private int cardCols;
    [SerializeField] private float horizontalDistance;
    [SerializeField] private float verticalDistance;
    [SerializeField] private float moveDelay;

    [Header("Card Prefabs")]
    [SerializeField] private MG5Card cardHPrefab;
    [SerializeField] private MG5Card cardDPrefab;
    [SerializeField] private MG5Card cardCPrefab;
    [SerializeField] private MG5Card cardSPrefab;

    [Header("Dialogue files")]
    [SerializeField] private TextAsset[] StartInkJSON;
    [SerializeField] private TextAsset[] OnReadyInkJSON;
    [SerializeField] private TextAsset[] OnDoneInkJSON;
    [SerializeField] private TextAsset[] EndInkJSON;
    
    
    private bool canChooseCards = false;
    private MG5Card currentChosenCard;

    private Day4DialogueManager talker;
    private DialogueController dialogueController;
    private static MiniGame5Manager instance;
    void Awake() {
        instance = this;

        if (cardRows * cardCols != 36) {
            Debug.LogError("Number of rows and columns in card grid is invalid");
        }
    }

    void Start() {
        dialogueController = DialogueController.GetInstance();
        talker = FindFirstObjectByType<Day4DialogueManager>();
        gameObject.SetActive(false);
    }

    public static MiniGame5Manager GetInstance() {
        return instance;
    }

    public bool CanChooseCards() {
        return canChooseCards;
    }

    public void GenerateCards() {
        for (int i = 0; i < 36; i++) {
            int type;
            while (true) {
                type = Random.Range(0, 4);

                if (type == 0 && cardsH.Count < 9) {
                    break;
                }
                else if (type == 1 && cardsD.Count < 9) {
                    break;
                }
                else if (type == 2 && cardsC.Count < 9) {
                    break;
                }
                else if (type == 3 && cardsS.Count < 9) {
                    break;
                }
            }

            switch (type) {
                case 0:
                    MG5Card cardH = Instantiate(cardHPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(4, 13);
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
                    MG5Card cardD = Instantiate(cardDPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(4, 13);
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
                    MG5Card cardC = Instantiate(cardCPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(4, 13);
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
                    MG5Card cardS = Instantiate(cardSPrefab, new Vector3(spawnPoint.transform.position.x, spawnPoint.transform.position.y + spawnGap * i, spawnPoint.transform.position.z), cardHPrefab.transform.rotation);
                    while (true) {
                        int index = Random.Range(4, 13);
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

    private bool CheckForCardInType(List<MG5Card> list, int index) {
        foreach (MG5Card c in list) {
            if (index == c.GetIndex()) {
                return true;
            }
        }

        return false;
    }

    public IEnumerator MoveCards() {
        for (int i = 0; i < cardRows; i++) {
            for (int j = 0; j < cardCols; j++) {
                MG5Card card = allCards[allCards.Count - 1 - (i * cardCols + j)].Find(x => x.GetGlobalIndex() == allCards.Count - 1 - (i * cardCols + j));
                StartCoroutine(card.MoveCard(new Vector3(destinationPoint.transform.position.x + j * horizontalDistance, destinationPoint.transform.position.y, destinationPoint.transform.position.z + i * verticalDistance), 0.01f));
                yield return new WaitForSeconds(moveDelay);
            }
        }

        Talk(OnReadyInkJSON[Random.Range(0, OnReadyInkJSON.Length - 1)]);
        canChooseCards = true;
    }

    public IEnumerator OnFlipped(MG5Card card) {
        canChooseCards = false;
        if (currentChosenCard == null) {
            currentChosenCard = card;
            canChooseCards = true;
        }
        else {
            if (currentChosenCard.GetIndex() == card.GetIndex()) {
                yield return new WaitForSeconds(1f);
                allCards[card.GetGlobalIndex()].Find(x => x.GetIndex() == card.GetIndex()).FindCard();
                allCards.Remove(card.GetGlobalIndex());
                allCards[currentChosenCard.GetGlobalIndex()].Find(x => x.GetIndex() == currentChosenCard.GetIndex()).FindCard();
                allCards.Remove(currentChosenCard.GetGlobalIndex());
                currentChosenCard = null;
                canChooseCards = true;
                if (IsGridEmpty()) {
                    EndMiniGame();
                }
            }
            else {
                yield return new WaitForSeconds(1f);
                currentChosenCard.Flip();
                currentChosenCard = null;
                card.Flip();
                canChooseCards = true;
            }
        }
    }

    private bool IsGridEmpty() {
        if (allCards.Count == 0) {
            return true;
        }

        return false;
    }


    private IEnumerator BeginTurn() {
        yield return new WaitForSeconds(spawnDelay);
        GenerateCards();
        StartCoroutine(MoveCards());
    }

    private void ResetCards() {
        foreach (MG5Card card in cardsH) {
            card.DestroyCard();
        }

        foreach (MG5Card card in cardsD) {
            card.DestroyCard();
        }

        foreach (MG5Card card in cardsC) {
            card.DestroyCard();
        }

        foreach (MG5Card card in cardsS) {
            card.DestroyCard();
        }

        allCards = new Dictionary<int, List<MG5Card>>();
        cardsH = new List<MG5Card>(9);
        cardsD = new List<MG5Card>(9);
        cardsC = new List<MG5Card>(9);
        cardsS = new List<MG5Card>(9);
    }

    public void StartMiniGame() {
        talker.Lock();
        gameObject.SetActive(true);
        ResetCards();
        Talk(StartInkJSON[Random.Range(0, StartInkJSON.Length - 1)]);
        StartCoroutine(BeginTurn());
    }
    private void EndMiniGame() {
        Talk(OnDoneInkJSON[Random.Range(0, OnDoneInkJSON.Length - 1)]);
        passedCount++;
        if (passedCount >= PassCount) {
            Talk(EndInkJSON[Random.Range(0, EndInkJSON.Length - 1)]);
        }
        else {
            StartMiniGame();
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

    public void UponExit()
    {
        return;
    }

    public void ChangeSprite(string spriteID) {
        talker.ChangeSprite(spriteID);
    }
}