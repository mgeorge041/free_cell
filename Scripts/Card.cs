using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    // Card properties
    private Suit cardSuit;
    private int cardValue;
    private CardColor cardColor;
    public bool isMoveable = false;

    // Train of cards
    private Card nextCard = null;
    private Card prevCard = null;
    private int numNextCards;

    // Card object items
    public Text cardValueText;
    public Image cardImage;
    private Sprite[] cardSprites;
    private Sprite originalSprite;
    public Sprite highlightSprite;

    // External objects
    private DropSpace parentDropSpace;
    private GameManager gameManager;

    // Initialize card properties
    public void Initialize (int cardValue, Suit suit) {
        this.cardValue = cardValue;
        cardValueText.text = cardValue.ToString();
        
        // Set card suit and color
        this.cardSuit = suit;
        if (suit == Suit.clubs || suit == Suit.spades) {
            cardColor = CardColor.black;
        }
        else {
            cardColor = CardColor.red;
        }

        // Set card sprite
        cardSprites = Resources.LoadAll<Sprite>("Card Art/");
        highlightSprite = cardSprites[4];
        switch (cardSuit) {
            case Suit.clubs:
                cardImage.sprite = cardSprites[0];
                break;
            case Suit.diamonds:
                cardImage.sprite = cardSprites[1];
                break;
            case Suit.hearts:
                cardImage.sprite = cardSprites[2];
                break;
            case Suit.spades:
                cardImage.sprite = cardSprites[3];
                break;
            default:
                break;
        }

        ResetCard();
    }

    // Reset card
    public void ResetCard() {
        nextCard = null;
        prevCard = null;
        isMoveable = false;
        numNextCards = 0;
    }

    // Get card value
    public int GetCardValue() {
        return cardValue;
    }

    // Get card color
    public CardColor GetCardColor() {
        return cardColor;
    }

    // Get card suit
    public Suit GetCardSuit() {
        return cardSuit;
    }

    // Get num next cards
    public int GetNumNextCards() {
        return numNextCards;
    }

    // Get next card
    public Card GetNextCard() {
        return nextCard;
    }

    // Set next and prev cards
    public static void SetNextPrevCard(Card prevCard, Card nextCard) {
        if (prevCard != null) {
            prevCard.SetNextCard(nextCard);
        }
        if (nextCard != null) {
            nextCard.SetPrevCard(prevCard);
        }
    }

    // Set next card
    public void SetNextCard(Card nextCard) {
        this.nextCard = nextCard;

        if (nextCard == null) {
            numNextCards = 0;
        }
        else if (IsNextCardInOrder(nextCard)) {
            numNextCards = nextCard.GetNumNextCards() + 1;
            nextCard.SetPrevCard(this);
        }
        else {
            numNextCards = 0;
            nextCard.SetPrevCard(this);
        }
    }

    // Determine whether next card is in order
    public bool IsNextCardInOrder(Card nextCard) {
        if (nextCard == null) {
            return true; 
        }
        if (cardColor != nextCard.cardColor && cardValue == nextCard.cardValue + 1) {
            return true;
        }
        return false;
    }

    // Determine whether next card is in order
    public bool IsNextCardInOrder() {
        if (nextCard == null) {
            return false;
        }

        if (cardColor != nextCard.cardColor && cardValue == nextCard.cardValue + 1) {
            return true;
        }
        return false;
    }

    // Get prev card
    public Card GetPrevCard() {
        return prevCard;
    }

    // Set prev card
    public void SetPrevCard(Card prevCard) {
        this.prevCard = prevCard;
    }

    // Determine whether prev card is in order
    public bool IsPrevCardInOrder(Card prevCard) {
        if (prevCard == null) {
            return true;
        }
        if (cardColor != prevCard.cardColor && cardValue == prevCard.cardValue - 1) {
            return true;
        }
        return false;
    }

    // Determine whether prev card is in order
    public bool IsPrevCardInOrder() {
        if (prevCard == null) {
            return false;
        }
        
        if (cardColor != prevCard.cardColor && cardValue == prevCard.cardValue - 1) {
            return true;
        }
        return false;
    }

    // Determine whether prev card is in suit order
    public bool IsPrevCardInSuitOrder(Card prevCard) {
        if (cardSuit == prevCard.GetCardSuit() && prevCard.GetCardValue() == cardValue - 1) {
            return true;
        }
        return false;
    }

    // Set prev cards immoveable
    public void SetPrevCardsImmovable() {
        if (prevCard != null) {
            prevCard.isMoveable = false;
            prevCard.SetPrevCardsImmovable();
        }
    }

    // Get parent drop space
    public DropSpace GetParentDropSpace() {
        return parentDropSpace;
    }

    // Remove card from parent drop space
    public void RemoveFromParentDropSpace() {
        parentDropSpace.RemoveCard(this);
        parentDropSpace.SetTrainMoveability(gameManager.GetNumMoveableCards());
    }

    // Set game manager
    public void SetGameManager(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Drag next card
    public void DragNextCard(PointerEventData pointerEventData, Vector2 yOffset) {
        if (nextCard != null) {
            Debug.Log("dragging next card");
            nextCard.transform.position = pointerEventData.position + yOffset;
            nextCard.DragNextCard(pointerEventData, yOffset + yOffset);
        }
    }

    // Set next card start position
    public void SetCardObjectParent() {
        transform.SetParent(gameManager.dragCardTransform);
        if (nextCard != null) {
            nextCard.SetCardObjectParent();
        }
    }

    // Set parent drop space
    public void SetParentDropSpace(DropSpace parentDropSpace) {
        this.parentDropSpace = parentDropSpace;
    }

    // Reset card position
    public void ResetCardPosition() {
        transform.SetParent(parentDropSpace.transform);
        if (nextCard != null) {
            nextCard.ResetCardPosition();
        }
    }

    // Set raycast target
    public void SetNextCardRaycastTarget(bool isRaycastTarget) {
        cardImage.raycastTarget = isRaycastTarget;
        if (nextCard != null) {
            nextCard.SetNextCardRaycastTarget(isRaycastTarget);
        }
    }

    // Mouse enter
    public void OnPointerEnter(PointerEventData pointerEventData) {
        if (nextCard != null) {
            Debug.Log("next card is not null");
        }
        if (isMoveable) {
            originalSprite = cardImage.sprite;
            cardImage.sprite = highlightSprite;
        }
    }

    // Mouse exit
    public void OnPointerExit(PointerEventData pointerEventData) {
        if (isMoveable) {
            cardImage.sprite = originalSprite;
        }
    }

    // Mouse drag
    public void OnDrag(PointerEventData pointerEventData) {
        transform.position = pointerEventData.position;
        DragNextCard(pointerEventData, new Vector2(0, -cardImage.sprite.rect.height / 4));
    }

    // Mouse down
    public void OnPointerDown(PointerEventData pointerEventData) {
        if (isMoveable) {
            SetCardObjectParent();
            SetNextCardRaycastTarget(false);
        }
    }

    // Mouse up
    public void OnPointerUp(PointerEventData pointerEventData) {
        SetNextCardRaycastTarget(true);
        DropSpace endingDropSpace = gameManager.GetEndingDropSpace(pointerEventData.position);
        if (endingDropSpace != null) {
            gameManager.MoveCard(this, endingDropSpace);
            bool movedCard = gameManager.MoveCard(this, endingDropSpace);
            if (!movedCard) {
                ResetCardPosition();
            }
        }
        else {
            ResetCardPosition();
        }
    }
}