using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class DropSpace : MonoBehaviour
{
    public GameManager gameManager;
    private List<Card> cards = new List<Card>();
    public RectTransform rectTransform;
    public BoxCollider2D boxCollider;
    public GameObject cardPlaceholder;
    public bool isFreeSpace;

    // Initialize
    public void Initialize(GameManager gameManager, bool isFreeSpace) {
        this.gameManager = gameManager;
        this.isFreeSpace = isFreeSpace;
    }

    // Get cards
    public List<Card> GetCards() {
        return cards;
    }

    // Get last card in list
    public Card GetLastCard() {
        if (cards.Count > 0) {
            return cards[cards.Count - 1];
        }
        return null;
    }

    // Add card to drop space
    public void AddCard(Card card) {        
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
        UpdateBoxColliderSize();
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

    // Get game manager
    public GameManager GetGameManager() {
        return gameManager;
    }

    // Set game manager
    public void SetGameManager(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Remove card from drop space
    public void RemoveCard(Card card) {
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
        UpdateBoxColliderSize();
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
