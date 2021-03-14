using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class CardTests {

        // Create test card (5 of clubs)
        public static Card CreateTestCard(int cardValue = 5, Suit suit = Suit.clubs) {
            Object cardPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Card.prefab", typeof(GameObject));
            GameObject cardObject = (GameObject)Object.Instantiate(cardPrefab);
            Card card = cardObject.GetComponent<Card>();
            card.Initialize(cardValue, suit);

            return card;
        }

        // Create train of cards
        public static Card CreateTestCardTrain() {
            Card newCard = CreateTestCard(5, Suit.clubs);
            Card nextCard = CreateTestCard(4, Suit.diamonds);
            Card thirdCard = CreateTestCard(3, Suit.clubs);

            // Add cards into train
            Card.SetNextPrevCard(newCard, nextCard);
            Card.SetNextPrevCard(nextCard, thirdCard);

            return newCard;
        }

        // Test card initializes
        [Test]
        public void CardInitializes() {
            Card card = CreateTestCard();

            // Confirm properties set correctly
            Assert.AreEqual(5, card.GetCardValue());
            Assert.AreEqual(Suit.clubs, card.GetCardSuit());
            Assert.AreEqual(CardColor.black, card.GetCardColor());
            Assert.AreEqual(0, card.GetNumNextCards());
            Assert.IsFalse(card.isMoveable);
        }

        // Test setting next and prev cards
        [Test]
        public void SetsNextPrevCards() {
            Card newCard = CreateTestCard(6, Suit.clubs);
            Card secondCard = CreateTestCard(6, Suit.diamonds);

            // Set next prev for two new cards
            Card.SetNextPrevCard(newCard, secondCard);

            // Confirm next and prev are set correctly
            Assert.IsNull(newCard.GetPrevCard());
            Assert.AreEqual(secondCard, newCard.GetNextCard());
            Assert.AreEqual(newCard, secondCard.GetPrevCard());
            Assert.IsNull(secondCard.GetNextCard());
        }

        // Test set next card
        [Test]
        public void SetsNextCard() {
            int cardValue = 1;
            Suit cardSuit = Suit.clubs;
            Card newCard = CreateTestCard(cardValue, cardSuit);

            // Set next card 1 higher same suit
            TestSetNextCard(cardValue + 1, cardSuit, 0);

            // Set next card 1 lower same suit
            TestSetNextCard(cardValue - 1, cardSuit, 0);

            // Set next card 1 higher different suit
            TestSetNextCard(cardValue + 1, Suit.diamonds, 0);

            // Set next card 1 lower different suit
            TestSetNextCard(cardValue - 1, Suit.diamonds, 1);

            // Test set next card
            void TestSetNextCard(int nextCardValue, Suit nextCardSuit, int numNextCards) {
                Card nextCard = CreateTestCard(nextCardValue, nextCardSuit);
                newCard.SetNextCard(nextCard);

                // Confirm next card is set and moveability is correct
                Assert.AreEqual(nextCard, newCard.GetNextCard());
                Assert.AreEqual(numNextCards, newCard.GetNumNextCards());
                Assert.AreEqual(newCard, nextCard.GetPrevCard());
            }
        }

        // Test set next card with train
        [Test]
        public void SetsNextCardTrain() {
            Card newCard = CreateTestCard(5, Suit.clubs);
            Card nextCard = CreateTestCard(4, Suit.diamonds);
            Card thirdCard = CreateTestCard(3, Suit.clubs);

            nextCard.SetNextCard(thirdCard);
            newCard.SetNextCard(nextCard);

            // Confirm train is set
            Assert.AreEqual(nextCard, newCard.GetNextCard());
            Assert.AreEqual(thirdCard, newCard.GetNextCard().GetNextCard());
            Assert.AreEqual(2, newCard.GetNumNextCards());

            // Confirm prev train is set
            Assert.AreEqual(nextCard, thirdCard.GetPrevCard());
            Assert.AreEqual(newCard, thirdCard.GetPrevCard().GetPrevCard());
        }

        // Test set prev card
        [Test]
        public void SetsPrevCard() {
            Card newCard = CreateTestCard(5, Suit.clubs);
            Card prevCard = CreateTestCard(9, Suit.diamonds);
            newCard.SetPrevCard(prevCard);

            // Confirm prev card is set correctly
            Assert.AreEqual(prevCard, newCard.GetPrevCard());
        }

        // Test move card
        [Test]
        public void MovesCard() {
            
            Card newCard = CreateTestCard(5, Suit.clubs);
            Card correctNextCard = CreateTestCard(4, Suit.diamonds);
            Card incorrectNextCard = CreateTestCard(4, Suit.clubs);


        }
    }
}
