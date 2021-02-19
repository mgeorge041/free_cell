using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Card> cards;
    private DropSpace dropSpace = null;
    public DropSpace[] dropSpaces;
    public GameObject dropSpacePrefab;
    public GameObject cardPrefab;
    public Transform dragCardTransform;
    public Transform playSectionTransform;

    // Set drop space
    public void SetDropSpace(DropSpace dropSpace) {
        this.dropSpace = dropSpace;
    }

    // Get drop space
    public DropSpace GetDropSpace() {
        return dropSpace;
    }

    // Shuffle cards
    public void ShuffleCards() {
        List<Card> shuffledCards = new List<Card>();
        for (int i = 0; i < 16; i++) {
            int randomInt = Random.Range(0, cards.Count);
            Card randomCard = cards[randomInt];
            shuffledCards.Add(randomCard);
            cards.Remove(randomCard);
        }
        cards = shuffledCards;
    }

    // Create deck
    private void CreateCards() {
        // Create cards
        cards = new List<Card>();
        CardColor[] cardColors = new CardColor[] {
            CardColor.red,
            CardColor.black
        };
        Suit[] suits = new Suit[] {
            Suit.clubs,
            Suit.diamonds,
            Suit.hearts,
            Suit.spades
        };
        for (int i = 0; i < 4; i++) {
            for (int j = 0; j < 4; j++) {

                // Create card
                GameObject newCardObject = Instantiate(cardPrefab);
                Card newCard = newCardObject.GetComponent<Card>();
                Suit cardSuit = suits[j];
                newCard.Initialize(i, cardSuit);
                newCard.SetGameManager(this);
                cards.Add(newCard);
            }
        }
    }

    // Reset game
    public void ResetGame() {

        // Clear drop space cards
        for (int i = 0; i < 8; i++) {
            dropSpaces[i].ClearCards();
        }

        // Randomly play cards
        ShuffleCards();
        int column = 0;
        for (int i = 0; i < cards.Count; i++) {
            dropSpaces[column].AddCard(cards[i]);
            column++;
            if (column >= 8) {
                column = 0;
            }
        }
    }

    // Start is called before the first frame update
    void Start() {
        dropSpaces = new DropSpace[8];
        // Create play section drop spaces
        for (int i = 0; i < 8; i++) {
            GameObject newDropSpaceObject = Instantiate(dropSpacePrefab);
            newDropSpaceObject.transform.SetParent(playSectionTransform);

            DropSpace newDropSpace = newDropSpaceObject.GetComponent<DropSpace>();
            dropSpaces[i] = newDropSpace;
            newDropSpace.SetGameManager(this);

        }
        CreateCards();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
