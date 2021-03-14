using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuitSpace : DropSpace
{
    protected List<Card> cards = new List<Card>();
    private Suit cardSuit;

    // Set card suit
    public void SetCardSuit(Suit cardSuit) {
        this.cardSuit = cardSuit;
    }

    // Add card to list
    public override void AddCard(Card card) {
        if (card == null) {
            return;
        }

        if (card.GetNextCard() == null) {
            cards.Add(card);
        }
    }

    // Remove card from list
    public override void RemoveCard(Card card) {
        cards.Remove(card);
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
        if (prevCard == null) {
            return true;
        }
        if (prevCard.GetCardColor() == nextCard.GetCardColor() && prevCard.GetCardValue() == nextCard.GetCardValue() - 1) {
            return true;
        }
        return false;
    }

    // Determine whether card can move to this space
    public override bool CanMoveToDropSpace(Card card) {
        // Empty suit space
        if (cards.Count == 0 && card.GetCardValue() == 1) {
            return true;
        }
        else if (cards.Count == 0 && card.GetCardValue() != 1) {
            return false;
        }

        // Other cards
        if (CardsInOrder(GetLastCard(), card)) {
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
        GetLastCard().isMoveable = true;
    }
}
