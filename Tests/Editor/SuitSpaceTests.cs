using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class SuitSpaceTests
    {
        // Create test suit space
        public static SuitSpace CreateTestSuitSpace() {
            SuitSpace newSuitSpace = DropSpace.CreateDropSpace<SuitSpace>();
            return newSuitSpace;
        }

        // Create test suit space with game manager
        public static SuitSpace CreateTestSuitSpaceWithGameManager() {
            SuitSpace newSuitSpace = DropSpace.CreateDropSpace<SuitSpace>();
            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            newSuitSpace.SetGameManager(gameManager);

            return newSuitSpace;
        }

        // Test create suit space
        [Test]
        public void CreatesSuitSpace() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();
            Assert.IsNotNull(newSuitSpace);
        }

        // Test add card
        [Test]
        public void AddsCard() {
            // Create suit space
            SuitSpace newSuitSpace = CreateTestSuitSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newSuitSpace.AddCard(newCard);

            // Confirm new card is added
            Assert.AreEqual(newCard, newSuitSpace.GetLastCard());
            Assert.AreEqual(1, newSuitSpace.GetNumCards());
        }

        // Test add cards
        [Test]
        public void AddsCards() {
            // Create suit space
            SuitSpace newSuitSpace = CreateTestSuitSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newSuitSpace.AddCard(newCard);
            Card secondCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newSuitSpace.AddCard(secondCard);

            // Confirm new card is added
            Assert.AreEqual(secondCard, newSuitSpace.GetLastCard());
            Assert.AreEqual(2, newSuitSpace.GetNumCards());
        }

        // Test add card train
        [Test]
        public void DoesNotAddCardTrain() {
            // Create suit space
            SuitSpace newSuitSpace = CreateTestSuitSpace();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newSuitSpace.AddCard(newCardTrain);

            // Confirm no cards added
            Assert.IsNull(newSuitSpace.GetLastCard());
            Assert.AreEqual(0, newSuitSpace.GetNumCards());
        }

        // Test remove card
        [Test]
        public void RemovesCards() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newSuitSpace.AddCard(newCard);

            // Remove card
            newSuitSpace.RemoveCard(newCard);

            // Confirm card removed
            Assert.IsNull(newSuitSpace.GetLastCard());
        }

        // Test move to empty space
        [Test]
        public void CanMoveToEmptySuitSpace() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();
            Card newCard = CardTests.CreateTestCard();
            Card aceCard = CardTests.CreateTestCard(1, Suit.diamonds);

            // Confirm non-ace card can't move to empty suit space
            bool canMove = newSuitSpace.CanMoveToDropSpace(newCard);
            Assert.IsFalse(canMove);

            // Confirm ace card can move to empty suit space
            canMove = newSuitSpace.CanMoveToDropSpace(aceCard);
            Assert.IsTrue(canMove);
        }


        // Test move to space with current card
        [Test]
        public void CanMoveToSuitSpaceWithCard() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();
            newSuitSpace.AddCard(newCard);

            // Confirm next card 1 lower different suit incorrect
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newSuitSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newSuitSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newSuitSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit correct
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newSuitSpace.CanMoveToDropSpace(nextCard);
            Assert.IsTrue(canMove);
        }


        // Test move card train to space
        [Test]
        public void CantMoveCardTrainToSuitSpace() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();
            Card newCard = CardTests.CreateTestCardTrain();

            // Confirm card can move to empty play space
            bool canMove = newSuitSpace.CanMoveToDropSpace(newCard);
            Assert.IsFalse(canMove);
        }


        // Test cards in order
        [Test]
        public void CardsInOrder() {
            SuitSpace newSuitSpace = CreateTestSuitSpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();

            // Confirm next card 1 lower different suit incorrect
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newSuitSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newSuitSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newSuitSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit correct
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newSuitSpace.CardsInOrder(newCard, nextCard);
            Assert.IsTrue(canMove);
        }
    }
}
