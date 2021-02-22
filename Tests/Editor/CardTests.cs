using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class CardTests {

        // Create test card
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
            nextCard.SetNextCard(thirdCard);
            newCard.SetNextCard(nextCard);

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
        }

        // Test next card in order
        [Test]
        public void NextCardInOrder() {
            int cardValue = 5;
            Card newCard = CreateTestCard(cardValue, Suit.clubs);

            // Confirm next card 1 lower different suit correct
            Card nextCard = CreateTestCard(cardValue - 1, Suit.diamonds);
            newCard.SetNextCard(nextCard);
            Assert.IsTrue(newCard.IsNextCardInOrder());

            // Confirm next card 1 higher different suit incorrect
            nextCard = CreateTestCard(cardValue + 1, Suit.diamonds);
            newCard.SetNextCard(nextCard);
            Assert.IsFalse(newCard.IsNextCardInOrder());

            // Confirm next card 1 lower same suit incorrect
            nextCard = CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            newCard.SetNextCard(nextCard);
            Assert.IsFalse(newCard.IsNextCardInOrder());

            // Confirm next card 1 higher same suit incorrect
            nextCard = CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            newCard.SetNextCard(nextCard);
            Assert.IsFalse(newCard.IsNextCardInOrder());

            // Confirm next card null incorrect
            newCard.SetNextCard(null);
            Assert.IsFalse(newCard.IsNextCardInOrder());
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

        // Test prev card in order
        [Test]
        public void PrevCardInOrder() {
            int cardValue = 5;
            Card newCard = CreateTestCard(cardValue, Suit.clubs);

            // Confirm prev card 1 lower different suit incorrect
            Card nextCard = CreateTestCard(cardValue - 1, Suit.diamonds);
            newCard.SetPrevCard(nextCard);
            Assert.IsFalse(newCard.IsPrevCardInOrder());

            // Confirm prev card 1 higher different suit correct
            nextCard = CreateTestCard(cardValue + 1, Suit.diamonds);
            newCard.SetPrevCard(nextCard);
            Assert.IsTrue(newCard.IsPrevCardInOrder());

            // Confirm prev card 1 lower same suit incorrect
            nextCard = CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            newCard.SetPrevCard(nextCard);
            Assert.IsFalse(newCard.IsPrevCardInOrder());

            // Confirm prev card 1 higher same suit incorrect
            nextCard = CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            newCard.SetPrevCard(nextCard);
            Assert.IsFalse(newCard.IsPrevCardInOrder());

            // Confirm next card null correct
            newCard.SetPrevCard(nextCard);
            Assert.IsTrue(newCard.IsNextCardInOrder(null));
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
