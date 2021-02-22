using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class DropSpaceTests
    {
        // Create test drop space
        public static DropSpace CreateTestDropSpace() {
            Object dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Drop Space.prefab", typeof(GameObject));
            GameObject newDropSpaceObject = (GameObject)Object.Instantiate(dropSpacePrefab);
            DropSpace newDropSpace = newDropSpaceObject.GetComponent<DropSpace>();

            return newDropSpace;
        }

        // Create test drop space with game manager
        public static DropSpace CreateTestDropSpaceWithGameManager() {
            Object dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Drop Space.prefab", typeof(GameObject));
            GameObject newDropSpaceObject = (GameObject)Object.Instantiate(dropSpacePrefab);
            DropSpace newDropSpace = newDropSpaceObject.GetComponent<DropSpace>();

            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            newDropSpace.SetGameManager(gameManager);

            return newDropSpace;
        }

        // Test create drop space
        [Test]
        public void CreatesDropSpace() {
            DropSpace newDropSpace = CreateTestDropSpace();
            Assert.IsNotNull(newDropSpace);
        }

        // Test add card
        [Test]
        public void AddsCard() {
            // Create drop space
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newDropSpace.AddCard(newCard);

            // Confirm new card is added and moveable
            Assert.AreEqual(newCard, newDropSpace.GetLastCard());
            Assert.IsTrue(newDropSpace.GetLastCard().isMoveable);
        }

        // Test add card train to empty drop space
        [Test]
        public void AddsCardTrain() {
            // Create drop space
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add train of cards
            Card newCard = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCard);

            // Confirm 3 cards added
            Assert.AreEqual(3, newDropSpace.GetCards().Count);

            // Confirm all cards are moveable
            foreach (Card card in newDropSpace.GetCards()) {
                Assert.IsTrue(card.isMoveable);
            }
        }

        // Test add card train to drop space with single card
        [Test]
        public void AddsCardTrainToSingleCard() {
            // Create drop space
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newDropSpace.AddCard(newCard);

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Confirm 3 cards added
            Assert.AreEqual(4, newDropSpace.GetCards().Count);
            Assert.AreEqual(newCard, newCardTrain.GetPrevCard());
            Assert.AreEqual(newCardTrain, newCard.GetNextCard());

            // Confirm all cards are moveable
            foreach (Card card in newDropSpace.GetCards()) {
                Assert.IsTrue(card.isMoveable);
            }
        }

        // Test add card train to drop space with single card and only 3 moveable cards
        [Test]
        public void AddsCardTrainToSingleMoveableCard() {
            // Create drop space
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();
            newDropSpace.GetGameManager().SetNumMoveableCards(3);

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newDropSpace.AddCard(newCard);

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Confirm last 3 cards are moveable
            Assert.IsFalse(newCard.isMoveable);
            Card cardTrain = newCardTrain;
            while (cardTrain != null) {
                Assert.IsTrue(cardTrain.isMoveable);
                cardTrain = cardTrain.GetNextCard();
            }
        }

        // Test remove card
        [Test]
        public void RemovesCards() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newDropSpace.AddCard(newCard);

            // Remove card
            newDropSpace.RemoveCard(newCard);

            // Confirm card removed
            Assert.IsNull(newDropSpace.GetLastCard());
            Assert.AreEqual(0, newDropSpace.GetCards().Count);
        }

        // Test remove cards from train
        [Test]
        public void RemovesCardsFromTrain() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Remove 2nd card in train
            newDropSpace.RemoveCard(newCardTrain.GetNextCard());

            // Confirm cards removed
            Assert.AreEqual(newCardTrain, newDropSpace.GetLastCard());
            Assert.AreEqual(1, newDropSpace.GetCards().Count);
            Assert.IsNull(newCardTrain.GetNextCard());
        }

        // Test remove all cards from drop space
        [Test]
        public void RemovesAllCards() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Remove all cards
            newDropSpace.RemoveCard(newCardTrain);

            // Confirms all cards removed
            Assert.IsNull(newDropSpace.GetLastCard());
            Assert.AreEqual(0, newDropSpace.GetCards().Count);
        }

        // Test move cards from one drop space to another
        [Test]
        public void MovesCardToNewEmptyDropSpace() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();
            DropSpace nextDropSpace = CreateTestDropSpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Move cards to next drop space
            newDropSpace.RemoveCard(newCardTrain);
            nextDropSpace.AddCard(newCardTrain);

            // Confirm cards removed
            Assert.IsNull(newDropSpace.GetLastCard());
            Assert.AreEqual(0, newDropSpace.GetCards().Count);
            Assert.AreEqual(3, nextDropSpace.GetCards().Count);
            Assert.IsNotNull(nextDropSpace.GetLastCard());
        }

        // Test move cards from one drop space to another with cards
        [Test]
        public void MovesCardToNewDropSpace() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();
            DropSpace nextDropSpace = CreateTestDropSpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newDropSpace.AddCard(newCardTrain);

            // Add card 
            Card newCard = CardTests.CreateTestCard(10, Suit.clubs);
            nextDropSpace.AddCard(newCard);
            newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            nextDropSpace.AddCard(newCard);

            // Move cards to next drop space
            newDropSpace.RemoveCard(newCardTrain);
            nextDropSpace.AddCard(newCardTrain);

            // Confirm cards removed
            Assert.IsNull(newDropSpace.GetLastCard());
            Assert.AreEqual(0, newDropSpace.GetCards().Count);
            Assert.AreEqual(5, nextDropSpace.GetCards().Count);
            Assert.IsNotNull(nextDropSpace.GetLastCard());
        }

        // Test box collider size update
        [Test]
        public void BoxColliderUpdatesSize() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();
            
            // Confirm initial size
            Assert.AreEqual(70, newDropSpace.GetComponent<RectTransform>().sizeDelta.y);

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newDropSpace.AddCard(newCard); 

            // Confirm still initial size and placeholder is deactivated
            Assert.AreEqual(70, newDropSpace.GetComponent<RectTransform>().rect.height);
            Assert.IsFalse(newDropSpace.cardPlaceholder.activeSelf);

            // Add second card
            Card nextCard = CardTests.CreateTestCard();
            newDropSpace.AddCard(nextCard);

            // Confirm size updates correctly
            Assert.AreEqual(3, newDropSpace.gameObject.transform.childCount);
            Assert.AreEqual(87.5, newDropSpace.GetComponent<RectTransform>().rect.height);
            
            // Remove both cards
            newDropSpace.RemoveCard(newCard);

            // Confirm size updates correctly
            Assert.AreEqual(70, newDropSpace.GetComponent<RectTransform>().rect.height);

            // Remove both cards
            newDropSpace.RemoveCard(newCard);

            // Confirm size updates correctly and placeholder activates
            Assert.AreEqual(70, newDropSpace.GetComponent<RectTransform>().rect.height);
            Assert.IsTrue(newDropSpace.cardPlaceholder.activeSelf);
        }
    }
}
