using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSpace : MonoBehaviour, IPointerEnterHandler
{
    public GameManager gameManager;
    private List<Card> cards = new List<Card>();
    public RectTransform rectTransform;

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
        // 0 cards, next = train, prev = null
        // > 0 cards, next = train, prev = cards[count - 1]
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
    }

    // Clear out cards
    public void ClearCards() {
        foreach (Card card in cards) {
            card.SetNextCard(null);
            card.SetPrevCard(null);
        }
        cards.Clear();
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
    }

    // Mouse enter
    public void OnPointerEnter(PointerEventData pointerEventData) {
        gameManager.SetDropSpace(this);
        foreach (Card card in cards) {
            Debug.Log("card in space: " + card.GetCardValue());
            Debug.Log("card in space: " + card.GetCardColor());
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
