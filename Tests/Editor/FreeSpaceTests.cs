using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class FreeSpaceTests
    {
        // Create test free space
        public static FreeSpace CreateTestFreeSpace() {
            FreeSpace newFreeSpace = DropSpace.CreateDropSpace<FreeSpace>();
            return newFreeSpace;
        }

        // Create test free space with game manager
        public static FreeSpace CreateTestFreeSpaceWithGameManager() {
            FreeSpace newFreeSpace = DropSpace.CreateDropSpace<FreeSpace>();
            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            newFreeSpace.SetGameManager(gameManager);

            return newFreeSpace;
        }

        // Test create free space
        [Test]
        public void CreatesFreeSpace() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();
            Assert.IsNotNull(newFreeSpace);
        }

        // Test add card
        [Test]
        public void AddsCard() {
            // Create free space
            FreeSpace newFreeSpace = CreateTestFreeSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newFreeSpace.AddCard(newCard);

            // Confirm new card is added
            Assert.AreEqual(newCard, newFreeSpace.GetLastCard());
            Assert.AreEqual(1, newFreeSpace.GetNumCards());
        }

        // Test add card train
        [Test]
        public void DoesNotAddCardTrain() {
            // Create free space
            FreeSpace newFreeSpace = CreateTestFreeSpace();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newFreeSpace.AddCard(newCardTrain);

            // Confirm no cards added
            Assert.IsNull(newFreeSpace.GetLastCard());
            Assert.AreEqual(0, newFreeSpace.GetNumCards());
        }

        // Test add card train to free space with single card
        [Test]
        public void LimitsToOneCard() {
            // Create free space
            FreeSpace newFreeSpace = CreateTestFreeSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newFreeSpace.AddCard(newCard);
            Card secondCard = CardTests.CreateTestCard();
            newFreeSpace.AddCard(secondCard);

            // Confirm no cards added
            Assert.AreEqual(newCard, newFreeSpace.GetLastCard());
            Assert.AreEqual(1, newFreeSpace.GetNumCards());
        }

        // Test remove card
        [Test]
        public void RemovesCards() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newFreeSpace.AddCard(newCard);

            // Remove card
            newFreeSpace.RemoveCard(newCard);

            // Confirm card removed
            Assert.IsNull(newFreeSpace.GetLastCard());
        }

        // Test move to empty space
        [Test]
        public void CanMoveToEmptyFreeSpace() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();
            Card newCard = CardTests.CreateTestCard();

            // Confirm card can move to empty free space
            bool canMove = newFreeSpace.CanMoveToDropSpace(newCard);
            Assert.IsTrue(canMove);
        }


        // Test move to space with current card
        [Test]
        public void CanMoveToFreeSpaceWithCard() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();
            newFreeSpace.AddCard(newCard);

            // Confirm next card 1 lower different suit incorrect
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newFreeSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newFreeSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newFreeSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newFreeSpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);
        }


        // Test move card train to space
        [Test]
        public void CantMoveCardTrainToFreeSpace() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();
            Card newCard = CardTests.CreateTestCardTrain();

            // Confirm card can move to empty free space
            bool canMove = newFreeSpace.CanMoveToDropSpace(newCard);
            Assert.IsFalse(canMove);
        }


        // Test cards in order
        [Test]
        public void CardsInOrder() {
            FreeSpace newFreeSpace = CreateTestFreeSpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();

            // Confirm next card 1 lower different suit incorrect
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newFreeSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newFreeSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newFreeSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newFreeSpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);
        }


        // Test move cards from one free space to another
        [Test]
        public void MovesCardToNewEmptyFreeSpace() {
            FreeSpace newFreeSpace = CreateTestFreeSpaceWithGameManager();
            FreeSpace nextFreeSpace = CreateTestFreeSpaceWithGameManager();

            // Add train of cards
            Card newCard = CardTests.CreateTestCard();
            newFreeSpace.AddCard(newCard);

            // Move card to next free space
            newFreeSpace.RemoveCard(newCard);
            nextFreeSpace.AddCard(newCard);

            // Confirm card removed
            Assert.IsNull(newFreeSpace.GetLastCard());
            Assert.IsNotNull(nextFreeSpace.GetLastCard());
            Assert.AreEqual(newCard, nextFreeSpace.GetLastCard());
        }
    }
}
