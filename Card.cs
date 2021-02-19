using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Card : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    public bool isMoveable = true;
    private Vector3 startPosition;
    public GameManager gameManager;
    private CardColor cardColor = CardColor.red;
    private Suit suit;
    private int cardValue = 1;
    public Text cardValueText;
    private DropSpace parentDropSpace;
    public Image cardImage;
    private Sprite[] cardSprites;
    private Sprite originalSprite;

    // Train of cards
    private Card nextCard;
    private Card prevCard;
    private int numNextCards;

    // Initialize card
    public void Initialize(int cardValue, Suit suit) {
        this.cardValue = cardValue;
        cardValueText.text = cardValue.ToString();

        if (suit == Suit.clubs || suit == Suit.spades) {
            cardColor = CardColor.black;
        }
        else {
            cardColor = CardColor.red;
        }

        cardSprites = Resources.LoadAll<Sprite>("Card Art/");
        switch (suit) {
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
    }

    // Get card value
    public int GetCardValue() {
        return cardValue;
    }

    // Get card color
    public CardColor GetCardColor() {
        return cardColor;
    }

    // Get next card
    public Card GetNextCard() {
        return nextCard;
    }

    // Set next card
    public void SetNextCard(Card nextCard) {
        this.nextCard = nextCard;
        if (IsNextCardInOrder(nextCard)) {
            Debug.Log("Next card is in order");
            isMoveable = true;
            if (nextCard != null) {
                Debug.Log("next card value: " + nextCard.cardValue);
                numNextCards = nextCard.numNextCards + 1;
            }
        }
        else {
            isMoveable = false;
            numNextCards = 0;
            SetPrevCardsImmovable();
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

    // Get prev card
    public Card GetPrevCard() {
        return prevCard;
    }

    // Set prev card
    public void SetPrevCard(Card prevCard) {
        this.prevCard = prevCard;
    }

    // Set prev cards immoveable
    public void SetPrevCardsImmovable() {
        if (prevCard != null) {
            prevCard.isMoveable = false;
            prevCard.SetPrevCardsImmovable();
        }
    }

    // Determine whether can move to drop space
    private bool CanMoveToDropSpace(DropSpace dropSpace) {
        Card lastCard = dropSpace.GetLastCard();
        if (lastCard != null) {
            Debug.Log("last card value: " + lastCard.cardValue);
            Debug.Log("last card color: " + lastCard.cardColor);
            Debug.Log("card value: " + cardValue);
            Debug.Log("Card color: " + cardColor);
        }

        // Determine whether can drop card on space
        if (lastCard == null || IsPrevCardInOrder(lastCard)) {
            Debug.Log("can move to drop space");
            return true;
        }
        return false;
    }

    // Set parent drop space
    public void SetParentDropSpace(DropSpace parentDropSpace) {
        this.parentDropSpace = parentDropSpace;
    }

    // Set game manager
    public void SetGameManager(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Mouse enter
    public void OnPointerEnter(PointerEventData pointerEventData) {
        Debug.Log("current card: " + cardValue);
        Debug.Log("current card: " + cardColor);
        if (nextCard != null) {
            Debug.Log("next card: " + nextCard.cardValue);
            Debug.Log("next card: " + nextCard.cardColor);
        }
        if (isMoveable) {
            originalSprite = cardImage.sprite;
            //cardImage.sprite = cardSprites[4];
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

    // Drag next card
    public void DragNextCard(PointerEventData pointerEventData, Vector2 yOffset) {
        if (nextCard != null) {
            nextCard.transform.position = pointerEventData.position + yOffset;
            nextCard.DragNextCard(pointerEventData, yOffset + yOffset);
        }
    }

    // Set next card start position
    public void SetNextCardStartPosition() {
        //startPosition = transform.position;
        transform.SetParent(gameManager.dragCardTransform);
        if (nextCard != null) {
            nextCard.SetNextCardStartPosition();
        }
    }

    // Reset next card
    public void ResetNextCardPosition() {
        //transform.position = startPosition;
        transform.SetParent(parentDropSpace.transform);
        if (nextCard != null) {
            nextCard.ResetNextCardPosition();
        }
    }

    // Mouse down
    public void OnPointerDown(PointerEventData pointerEventData) {
        if (isMoveable) {
            SetNextCardStartPosition();
            cardImage.raycastTarget = false;
        }
    }

    // Mouse up
    public void OnPointerUp(PointerEventData pointerEventData) {
        cardImage.raycastTarget = true;
        DropSpace currentDropSpace = gameManager.GetDropSpace();
        if (currentDropSpace != null) {
            if (CanMoveToDropSpace(currentDropSpace)) {
                parentDropSpace.RemoveCard(this);
                currentDropSpace.AddCard(this);
            }
            else {
                ResetNextCardPosition();
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
