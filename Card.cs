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
    private Card nextCard;

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

    // Set next card
    public void SetNextCard(Card nextCard) {
        this.nextCard = nextCard;
        if (IsNextCardInOrder(nextCard)) {
            isMoveable = true;
        }
        else {
            isMoveable = false;
        }
    }

    // Get next card
    public Card GetNextCard() {
        return nextCard;
    }

    // Determine whether next card is moveable
    public bool IsNextCardInOrder(Card nextCard) {
        if (nextCard == null) {
            return true; 
        }
        if (cardColor != nextCard.cardColor && cardValue == nextCard.cardValue + 1) {
            return true;
        }
        return false;
    }

    // Determine whether can move to drop space
    private bool CanMoveToDropSpace(DropSpace dropSpace) {
        Card lastCard = dropSpace.GetLastCard();
        if (lastCard != null) {
            Debug.Log("last card value: " + lastCard.cardValue);
        }

        // Determine whether can drop card on space
        if (lastCard == null || IsNextCardInOrder(lastCard)) {
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
        if (isMoveable) {
            originalSprite = cardImage.sprite;
            cardImage.sprite = cardSprites[4];
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
        DragNextCard(pointerEventData);
    }

    // Drag next card
    public void DragNextCard(PointerEventData pointerEventData) {
        if (nextCard != null) {
            nextCard.transform.position = pointerEventData.position + new Vector2(0, -cardImage.sprite.rect.height / 2);
            nextCard.DragNextCard(pointerEventData);
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
        }
    }

    // Mouse up
    public void OnPointerUp(PointerEventData pointerEventData) {
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
