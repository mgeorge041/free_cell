using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpace : DropSpace
{
    protected List<Card> cards = new List<Card>();
    public RectTransform rectTransform;
    public GameObject cardPlaceholder;

    // Get cards
    public List<Card> GetCards() {
        return cards;
    }

    // Get last card in list
    public override Card GetLastCard() {
        if (cards.Count > 0) {
            return cards[cards.Count - 1];
        }
        return null;
    }

    // Add card to drop space
    public override void AddCard(Card card) {
        if (card == null) {
            return;
        }

        // Deactivate card placeholder
        cardPlaceholder.SetActive(false);

        // Set next and prev cards
        if (cards.Count > 0) {
            cards[cards.Count - 1].SetNextCard(card);
            card.SetPrevCard(cards[cards.Count - 1]);
        }
        else {
            card.SetPrevCard(null);
        }

        // Add all cards in train
        while (card != null) {
            cards.Add(card);
            card.transform.SetParent(transform);
            card.SetParentDropSpace(this);
            card = card.GetNextCard();
        }

        // Update train moveability
        SetTrainMoveability(gameManager.GetNumMoveableCards());

        // Update box collider size
        //UpdateBoxColliderSize();
    }

    // Clear out cards
    public void ClearCards() {
        foreach (Card card in cards) {
            card.ResetCard();
        }
        cards.Clear();
    }

    // Determine moveability for stack
    public void SetTrainMoveability(int numMoveableCards) {
        Card currentCard = GetLastCard();
        currentCard.isMoveable = true;

        // Move up prev train
        int numCardsMoved = 1;
        while (currentCard != null) {
            if (currentCard.IsPrevCardInOrder()) {
                if (numCardsMoved >= numMoveableCards) {
                    currentCard.GetPrevCard().isMoveable = false;
                }
                else {
                    currentCard.GetPrevCard().isMoveable = true;
                }
                currentCard = currentCard.GetPrevCard();
            }
            else {
                break;
            }
            numCardsMoved++;
        }
    }

    // Update box collider size
    public void UpdateBoxColliderSize() {
        RectTransform rectTransform = gameObject.GetComponent<RectTransform>();
        float height = (float)(70 + 17.5 * (Mathf.Max(cards.Count - 1, 0)));
        rectTransform.sizeDelta = new Vector2(50, height);
        boxCollider.size = rectTransform.sizeDelta;
    }

    // Remove card from drop space
    public override void RemoveCard(Card card) {
        while (card != null) {
            cards.Remove(card);
            card = card.GetNextCard();
        }
        if (cards.Count > 0) {
            cards[cards.Count - 1].SetNextCard(null);
        }
        else {
            cardPlaceholder.SetActive(true);
        }
        //UpdateBoxColliderSize();
    }

    // Determine whether can move to drop space
    public override bool CanMoveToDropSpace(Card card) {
        Card lastCard = GetLastCard();
        if (lastCard != null) {
            Debug.Log("last card: " + lastCard.GetCardValue() + ", " + lastCard.GetCardColor());
            Debug.Log("card: " + card.GetCardValue() + ", " + card.GetCardColor());
        }

        // Determine whether can drop card on space
        if (lastCard == null || card.IsPrevCardInOrder(lastCard)) {
            Debug.Log("can move to drop space");
            return true;
        }
        return false;
    }
}
