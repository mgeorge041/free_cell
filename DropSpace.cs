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
        if (cards.Count > 0) {
            cards[cards.Count - 1].SetNextCard(card);
        }
        cards.Add(card);

        // Position card
        //float yOffset = -((cards.Count - 1) * 35) / 2;
        //rectTransform.sizeDelta = new Vector2(50, Mathf.Max(yOffset + 70, 70));
        card.transform.SetParent(transform);
        //card.transform.position = new Vector2(transform.position.x, transform.position.y + yOffset - 35);
        card.SetParentDropSpace(this);

        // Position next card of card
        AddCard(card.GetNextCard());
    }

    // Clear out cards
    public void ClearCards() {
        foreach (Card card in cards) {
            card.SetNextCard(null);
        }
        cards.Clear();
    }

    // Set game manager
    public void SetGameManager(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Remove card from drop space
    public void RemoveCard(Card card) {
        cards.Remove(card);
    }

    // Mouse enter
    public void OnPointerEnter(PointerEventData pointerEventData) {
        gameManager.SetDropSpace(this);
        Debug.Log("Setting drop space");
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
