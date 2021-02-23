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
            FreeSpace newFreeSpace = CreateTestFreeSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newFreeSpace.AddCard(newCard);

            // Confirm new card is added and moveable
            Assert.AreEqual(newCard, newFreeSpace.GetLastCard());
            Assert.IsTrue(newFreeSpace.GetLastCard().isMoveable);
        }

        // Test add card train to empty free space
        [Test]
        public void AddsCardTrain() {
            // Create free space
            FreeSpace newFreeSpace = CreateTestFreeSpaceWithGameManager();

            // Add train of cards
            Card newCard = CardTests.CreateTestCardTrain();
            newFreeSpace.AddCard(newCard);

            // Confirm no cards added
            Assert.IsNull(newFreeSpace.GetLastCard());
        }

        // Test add card train to free space with single card
        [Test]
        public void AddsCardTrainToSingleCard() {
            // Create free space
            FreeSpace newFreeSpace = CreateTestFreeSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newFreeSpace.AddCard(newCard);

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newFreeSpace.AddCard(newCardTrain);

            // Confirm no cards added
            Assert.AreEqual(newCard, newFreeSpace.GetLastCard());
            Assert.IsNull(newCard.GetNextCard());
        }

        // Test remove card
        [Test]
        public void RemovesCards() {
            FreeSpace newFreeSpace = CreateTestFreeSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newFreeSpace.AddCard(newCard);

            // Remove card
            newFreeSpace.RemoveCard(newCard);

            // Confirm card removed
            Assert.IsNull(newFreeSpace.GetLastCard());
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
