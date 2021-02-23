using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeSpace : DropSpace
{
    private Card card;

    // Add card to free space
    public override void AddCard(Card card) {
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

    // Get card
    public override Card GetLastCard() {
        return card;
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
}
