using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MG6Deck : MonoBehaviour
{
    [SerializeField] private Transform deckPosition;
    [SerializeField] private Animator animator;
    [SerializeField] private bool isPlayerDeck;
    [Header("Settings")]
    [SerializeField] private float maxDistance;
    [SerializeField] private float defDistance;
    [SerializeField] private float minDistance;
    [SerializeField] private bool moveX;
    [SerializeField] private bool moveY;
    [SerializeField] private bool moveZ;
    [SerializeField] private float CardGap;
    [SerializeField] private float maxAngle;
    [SerializeField] private float startAngle;
    [SerializeField] private float defAngle;
    [SerializeField] private float minAngle;
    [SerializeField] private bool rotateX;
    [SerializeField] private bool rotateY;
    [SerializeField] private bool rotateZ;
    [SerializeField] private bool aroundX;
    [SerializeField] private bool aroundY;
    [SerializeField] private bool aroundZ;
    [SerializeField] private float aroundXAngle;
    [SerializeField] private float aroundYAngle;
    [SerializeField] private float aroundZAngle;
    private List<MG6Card> cards = new List<MG6Card>();
    private MG6Card selectedCard;

    private bool isLocked = false;
    private bool canMakeSelection = false;


    public bool CanMakeSelection() { return canMakeSelection; }

    public void AddCard(MG6Card card) {
        Vector3 localPosition = card.transform.position;
        Quaternion localRotation = card.transform.rotation;
        Vector3 localScale = card.transform.localScale;
        card.transform.parent = deckPosition.transform;
        card.transform.position = localPosition;
        card.transform.rotation = localRotation;
        card.transform.localScale = localScale;
        cards.Add(card);
        card.AssignDeck(this);
        UpdateDeck();
    }

    public void RemoveCard(MG6Card card) {
        cards.Remove(card);
        if (MiniGame6Manager.GetInstance().IsSelectionStage()) {
            card.DestroyCard();
        }

        UpdateDeck();
    }

    public void UpdateDeck() {
        if (cards.Count == 0) {
            return;
        }

        // Calculate distance between cards
        float basedistance = defDistance;
        while (Mathf.Abs(cards.Count * basedistance) > maxDistance && basedistance > minDistance) {
            basedistance -= minDistance / cards.Count;
        }

        // Calculate angle between cards
        float baseangle = defAngle;
        while (Mathf.Abs(cards.Count * baseangle) > maxAngle && baseangle > minAngle) {
            baseangle -= minAngle / cards.Count;
        }

        float distance = -((basedistance * cards.Count / 2f) - (basedistance / 2f));
        float angle = startAngle - (baseangle * cards.Count / 2f) + (baseangle / 2f);
        
        float gap = 0f;
        StopCoroutine("MoveCard");
        foreach (MG6Card card in cards) {
            StartCoroutine(card.MoveCard(new Vector3(
                moveX ? deckPosition.position.x + distance : moveZ ? deckPosition.position.x + gap : deckPosition.position.x,
                moveY ? deckPosition.position.y + distance : deckPosition.position.y,
                moveZ ? deckPosition.position.z + distance : moveX ? deckPosition.position.z + gap : deckPosition.position.z
            ), 0.01f));
            distance += basedistance;
            gap += CardGap;
            card.RotateCard(new Vector3(
                aroundX ? aroundXAngle : (rotateX ? angle : 0),
                aroundY ? aroundYAngle : (rotateY ? angle : 0),
                aroundZ ? aroundZAngle : (rotateZ ? angle : 0)
            ));
            angle += baseangle;
        }
    }

    public void OnCardInteraction(MG6Card card, bool isPlayer) {
        if (isPlayer && !canMakeSelection) return;

        if (selectedCard == null) {
            selectedCard = card;
            card.SelectCard();
        }
        else {
            if (MiniGame6Manager.GetInstance().IsSelectionStage()) {
                if (card.GetIndex() == selectedCard.GetIndex()) {
                    if (card == selectedCard) {
                        selectedCard.UnSelectCard();
                        selectedCard = null;
                    }
                    else {
                        RemoveCard(card);
                        RemoveCard(selectedCard);
                        selectedCard = null;
                        if (!ContainsPairs()) {
                            MiniGame6Manager.GetInstance().FinishSelectionStage(isPlayer);
                        }
                    }
                }
                else {
                    selectedCard.UnSelectCard();
                    selectedCard = card;
                    card.SelectCard();
                }
            }
            else {
                if (selectedCard == card) {
                    selectedCard.ToDefault();
                    selectedCard = null;
                    MiniGame6Manager.GetInstance().CardPicked(card, this);
                }
                else {
                    selectedCard.UnSelectCard();
                    selectedCard = card;
                    card.SelectCard();
                }
            }
        }
    }

    public void Lock() {
        canMakeSelection = false;
    }

    public void Unlock() {
        canMakeSelection = true;
    }

    public bool ContainsPairs() {
        foreach (MG6Card card in cards) {
            foreach (MG6Card other in cards) {
                if (card.GetIndex() == other.GetIndex() && card != other) {
                    return true;
                }
            }
        }

        return false;
    }

    public bool IsDeckEmpty() { return cards.Count == 0; }

    public bool CheckLosingCondition() {
        if (cards.Count == 1 && cards[0].GetIndex() == 10) {
            return true;
        }

        return false;
    }

    public void Reset() {
        foreach(MG6Card card in cards) {
            card.DestroyCard();
        }
        cards.Clear();
        selectedCard = null;
        canMakeSelection = false;
    }

    public void ShuffleDeck() {
        for (int i = 0; i < cards.Count; i++) {
            int randomIndex = Random.Range(0, cards.Count);
            MG6Card temp = cards[i];
            cards[i] = cards[randomIndex];
            cards[randomIndex] = temp;
        }

        UpdateDeck();
    }

    public IEnumerator ShowDeck() {
        while (true) {
            if (HasMovingCards()) {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            else {
                break;
            }
        }
        animator.SetTrigger("ShowDeck");
        Unlock();
    }

    private bool HasMovingCards() {
        foreach (MG6Card card in cards) {
            if (card.IsMoving()) {
                return true;
            }
        }

        return false;
    }
    
    public IEnumerator HideDeck() {
        Lock();
        while (true) {
            if (HasMovingCards()) {
                yield return new WaitForSeconds(0.1f);
                continue;
            }
            else {
                break;
            }
        }
        animator.SetTrigger("HideDeck");
    }

    public bool IsLocked() { return isLocked; }

    // ===================================== AI =========================================== //

    public IEnumerator FindPairs() {
        if (!ContainsPairs()) {
            MiniGame6Manager.GetInstance().FinishSelectionStage(false);
            yield break;
        }
        yield return new WaitForSeconds(1f);
        while (ContainsPairs()) {
            foreach (MG6Card card in cards) {
                card.PseudoInteract();
                yield return new WaitForSeconds(1f);

                MG6Card pair = FindPair(card);
                if (pair != null) {
                    pair.PseudoInteract();
                    break;
                }
            }
        }
        
    }

    private MG6Card FindPair(MG6Card card) {
        foreach (MG6Card other in cards) {
            if (card.GetIndex() == other.GetIndex() && card != other) {
                return other;
            }
        }

        return null;
    }

    public void PickRandomCard() {
        int randomIndex;
        while (true) {
            randomIndex = Random.Range(0, cards.Count);
            if (cards.Count == 1) {
                break;
            }
            if (cards[randomIndex] == selectedCard) { continue; }
            break;
        }

        cards[randomIndex].PseudoInteract();
    }

    public void SubmitPick() {
        selectedCard.PseudoInteract();
    }

    public bool HasQueen() {
        foreach (MG6Card card in cards) {
            if (card.GetIndex() == 10) {
                return true;
            }
        }

        return false;
    }
}
