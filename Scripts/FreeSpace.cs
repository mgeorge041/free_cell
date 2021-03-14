using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpace : DropSpace
{
    private Card card;

    // Add card to free space
    public override void AddCard(Card card) {
        if (this.card != null) {
            return;
        }

        if (card.GetNextCard() == null) {
            this.card = card;
        }
        card.transform.SetParent(transform);
        card.SetParentDropSpace(this);
    }

    // Remove card from free space
    public override void RemoveCard(Card card) {
        this.card = null;
    }

    // Clear out cards
    public override void ClearCards() {
        card = null;
    }

    // Get card
    public override Card GetLastCard() {
        return card;
    }

    // Get number of cards
    public override int GetNumCards() {
        if (card == null) {
            return 0;
        }
        return 1;
    }

    // Determine whether card is in order
    public override bool CardsInOrder(Card prevCard, Card nextCard) {
        return false;
    }

    // Determine whether can move to free space
    public override bool CanMoveToDropSpace(Card card) {
        if (this.card != null) {
            return false;
        }
        else if (card.GetNumNextCards() == 0) {
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
        card.isMoveable = true;
    }
}
