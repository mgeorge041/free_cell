using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySpace : DropSpace
{
    protected List<Card> cards = new List<Card>();
    public RectTransform rectTransform;

    // Get cards
    public List<Card> GetCards() {
        return cards;
    }

    // Add card to play space
    public override void AddCard(Card card) {
        if (card == null) {
            return;
        }

        // Add card train
        while (card != null) {
            Card.SetNextPrevCard(GetLastCard(), card);
            cards.Add(card);
            card.transform.SetParent(transform);
            card.SetParentDropSpace(this);
            card = card.GetNextCard();
        }
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
        //UpdateBoxColliderSize();
    }

    // Clear out cards
    public override void ClearCards() {
        cards.Clear();
    }

    // Get last card in list
    public override Card GetLastCard() {
        if (cards.Count > 0) {
            return cards[cards.Count - 1];
        }
        return null;
    }

    // Get number of cards
    public override int GetNumCards() {
        return cards.Count;
    }

    // Determine whether card is in order
    public override bool CardsInOrder(Card prevCard, Card nextCard) {
        if (prevCard.GetCardColor() != nextCard.GetCardColor() && prevCard.GetCardValue() == nextCard.GetCardValue() + 1) {
            return true;
        }
        return false;
    }

    // Determine whether can move to drop space
    public override bool CanMoveToDropSpace(Card card) {
        Card lastCard = GetLastCard();
        if (lastCard != null) {
            Debug.Log("last card: " + lastCard.GetCardValue() + ", " + lastCard.GetCardColor());
            Debug.Log("card: " + card.GetCardValue() + ", " + card.GetCardColor());
        }

        // Determine whether can drop card on space
        if (lastCard == null || CardsInOrder(lastCard, card)) {
            Debug.Log("can move to drop space");
            return true;
        }
        return false;
    }

    // Move card to drop space
    public override void MoveCardToDropSpace(Card card) {
        card.RemoveFromParentDropSpace();
        AddCard(card);
    }

    // Determine moveability for stack
    public override void SetTrainMoveability(int numMoveableCards) {
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
}
