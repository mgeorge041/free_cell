using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Cards
    int numCardValues = 13;
    int numCardSuits = 4;
    private List<Card> cards;
    public GameObject cardPrefab;

    // Drop spaces
    int numPlaySpaces = 8;
    int numFreeSpaces = 4;
    public PlaySpace[] playSpaces;
    public FreeSpace[] freeSpaces;
    public SuitSpace[] suitSpaces;
    
    // Area transforms
    public Transform dragCardTransform;
    public Transform playSectionTransform = null;
    public Transform freeSectionTransform = null;
    public Transform suitSectionTransform = null;

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
        for (int i = 0; i < numPlaySpaces; i++) {
            if (playSpaces[i].Contains(mousePosition)) {
                Debug.Log("returning play space");
                return playSpaces[i];
            }
        }
        for (int i = 0; i < numFreeSpaces; i++) {
            if (freeSpaces[i].Contains(mousePosition)) {
                Debug.Log("returning free space");
                return freeSpaces[i];
            }
        }
        return null;
    }

    // Shuffle cards
    public void ShuffleCards() {
        List<Card> shuffledCards = new List<Card>();
        for (int i = 0; i < (numCardValues * numCardSuits); i++) {
            int randomInt = Random.Range(0, cards.Count);
            Card randomCard = cards[randomInt];
            shuffledCards.Add(randomCard);
            cards.Remove(randomCard);
        }
        cards = shuffledCards;
    }

    // Create deck
    public void CreateCards() {
        // Create cards
        cards = new List<Card>();
        Suit[] suits = new Suit[] {
            Suit.clubs,
            Suit.diamonds,
            Suit.hearts,
            Suit.spades
        };
        for (int i = 0; i < numCardValues; i++) {
            for (int j = 0; j < numCardSuits; j++) {

                // Create card
                GameObject newCardObject = Instantiate(cardPrefab);
                Card newCard = newCardObject.GetComponent<Card>();
                newCard.Initialize(i, suits[j]);
                newCard.SetGameManager(this);
                cards.Add(newCard);
            }
        }
    }

    // Get deck
    public List<Card> GetCards() {
        return cards;
    }

    // Deal cards
    public void DealCards() {
        ShuffleCards();
        int column = 0;
        for (int i = 0; i < cards.Count; i++) {
            playSpaces[column].AddCard(cards[i]);
            column++;
            if (column >= numPlaySpaces) {
                column = 0;
            }
        }
    }

    // Create drop spaces
    public void CreateDropSpaces() {
        // Create play section drop spaces
        playSpaces = new PlaySpace[8];
        for (int i = 0; i < 8; i++) {
            PlaySpace newPlaySpace = DropSpace.CreateDropSpace<PlaySpace>();
            newPlaySpace.transform.SetParent(playSectionTransform);
            playSpaces[i] = newPlaySpace;
            newPlaySpace.Initialize(this);
        }

        // Create free section drop spaces
        freeSpaces = new FreeSpace[4];
        for (int i = 0; i < 4; i++) {
            FreeSpace newFreeSpace = DropSpace.CreateDropSpace<FreeSpace>();
            newFreeSpace.transform.SetParent(freeSectionTransform);
            freeSpaces[i] = newFreeSpace;
            newFreeSpace.Initialize(this);
        }

        // Create suit section drop spaces
        suitSpaces = new SuitSpace[4];
        for (int i = 0; i < 4; i++) {
            SuitSpace newSuitSpace = DropSpace.CreateDropSpace<SuitSpace>();
            newSuitSpace.transform.SetParent(suitSectionTransform);
            suitSpaces[i] = newSuitSpace;
            newSuitSpace.Initialize(this);
        }
    }

    // Get play spaces
    public PlaySpace[] GetPlaySpaces() {
        return playSpaces;
    }

    // Get play spaces
    public FreeSpace[] GetFreeSpaces() {
        return freeSpaces;
    }

    // Get play spaces
    public SuitSpace[] GetSuitSpaces() {
        return suitSpaces;
    }

    // Reset game
    public void ResetGame() {

        // Clear drop space cards
        for (int i = 0; i < numPlaySpaces; i++) {
            playSpaces[i].ClearCards();
        }

        // Randomly play cards
        DealCards();

        // Set moveability for cards in piles
        for (int i = 0; i < numPlaySpaces; i++) {
            playSpaces[i].SetTrainMoveability(numMoveableCards);
        }
    }

    // Move card from one drop space to another
    public bool MoveCard(Card card, DropSpace dropSpace) {
        if (dropSpace.CanMoveToDropSpace(card)) {
            dropSpace.MoveCardToDropSpace(card);
            return true;
        }
        return false;
    }

    // Initializes game manager
    public void Initialize() {
        CreateDropSpaces();
        CreateCards();
        ResetGame();
    }

    // Start is called before the first frame update
    void Start() {
        Initialize();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
