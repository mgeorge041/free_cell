using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Cards
    private List<Card> cards;
    public GameObject cardPrefab;

    // Drop spaces
    int numPlayDropSpaces = 8;
    public DropSpace[] playDropSpaces;
    public DropSpace[] freeDropSpaces;
    public GameObject dropSpacePrefab;
    public GameObject freeSpacePrefab;
    
    // Area transforms
    public Transform dragCardTransform;
    public Transform playSectionTransform;
    public Transform freeSectionTransform;

    // Player variables
    int numMoveableCards = 5;
    int score;

    // Get number of moveable cards
    public int GetNumMoveableCards() {
        return numMoveableCards;
    }

    // Set number of moveable cards
    public void SetNumMoveableCards(int numMoveableCards) {
        this.numMoveableCards = numMoveableCards;
    }

    // Get ending drop space
    public DropSpace GetEndingDropSpace(Vector2 mousePosition) {
        for (int i = 0; i < numPlayDropSpaces; i++) {
            if (playDropSpaces[i].boxCollider.OverlapPoint(mousePosition)) {
                Debug.Log("returning drop space");
                return playDropSpaces[i];
            }
        }
        return null;
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
                newCard.Initialize(i, suits[j]);
                newCard.SetGameManager(this);
                cards.Add(newCard);
            }
        }
    }

    // Reset game
    public void ResetGame() {

        // Clear drop space cards
        for (int i = 0; i < numPlayDropSpaces; i++) {
            playDropSpaces[i].ClearCards();
        }

        // Randomly play cards
        ShuffleCards();
        int column = 0;
        for (int i = 0; i < cards.Count; i++) {
            playDropSpaces[column].AddCard(cards[i]);
            column++;
            if (column >= 8) {
                column = 0;
            }
        }

        // Set moveability for cards in piles
        for (int i = 0; i < numPlayDropSpaces; i++) {
            playDropSpaces[i].SetTrainMoveability(numMoveableCards);
        }
    }

    // Start is called before the first frame update
    void Start() {
        playDropSpaces = new DropSpace[8];
        // Create play section drop spaces
        for (int i = 0; i < 8; i++) {
            GameObject newDropSpaceObject = Instantiate(dropSpacePrefab);
            newDropSpaceObject.transform.SetParent(playSectionTransform);

            DropSpace newDropSpace = newDropSpaceObject.GetComponent<DropSpace>();
            playDropSpaces[i] = newDropSpace;
            newDropSpace.Initialize(this, false);

        }

        // Create free section drop spaces
        freeDropSpaces = new DropSpace[4];
        for (int i = 0; i < 4; i++) {
            GameObject newFreeSpaceObject = Instantiate(freeSpacePrefab);
            newFreeSpaceObject.transform.SetParent(freeSectionTransform);

            DropSpace newDropSpace = newFreeSpaceObject.GetComponent<DropSpace>();
            freeDropSpaces[i] = newDropSpace;
            newDropSpace.Initialize(this, true);

        }
        CreateCards();
        ResetGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
