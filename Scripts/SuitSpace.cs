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
        while (card != null) {
            cards.Add(card);
            card.isMoveable = false;
            card = card.GetNextCard();
        }
        GetLastCard().isMoveable = true;
    }

    // Determine whether card can move to this space
    public override bool CanMoveToDropSpace(Card card) {
        if (card.GetCardSuit() == cardSuit && card.IsPrevCardInSuitOrder(card)) {
            return true;
        }
        return false;
    }

    // Get last card in list
    public override Card GetLastCard() {
        if (cards.Count > 0) {
            return cards[cards.Count - 1];
        }
        return null;
    }

    // Remove card from list
    public override void RemoveCard(Card card) {
        cards.Remove(card);
    }
}
