using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlaySpaceTests
    {
        // Create test play space
        public static PlaySpace CreateTestPlaySpace() {
            PlaySpace newPlaySpace = DropSpace.CreateDropSpace<PlaySpace>();
            return newPlaySpace;
        }

        // Create test play space with game manager
        public static PlaySpace CreateTestPlaySpaceWithGameManager() {
            PlaySpace newPlaySpace = DropSpace.CreateDropSpace<PlaySpace>();
            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            newPlaySpace.SetGameManager(gameManager);

            return newPlaySpace;
        }

        // Test create play space
        [Test]
        public void CreatesPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();
            Assert.IsNotNull(newPlaySpace);
        }

        // Test add card
        [Test]
        public void AddsCard() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);

            // Confirm new card is added and moveable
            Assert.AreEqual(newCard, newPlaySpace.GetLastCard());
            Assert.IsTrue(newPlaySpace.GetLastCard().isMoveable);
        }

        // Test add card train to empty play space
        [Test]
        public void AddsCardTrain() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add train of cards
            Card newCard = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCard);

            // Confirm 3 cards added
            Assert.AreEqual(3, newPlaySpace.GetCards().Count);

            // Confirm all cards are moveable
            foreach (Card card in newPlaySpace.GetCards()) {
                Assert.IsTrue(card.isMoveable);
            }
        }

        // Test add card train to play space with single card
        [Test]
        public void AddsCardTrainToSingleCard() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newPlaySpace.AddCard(newCard);

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Confirm 3 cards added
            Assert.AreEqual(4, newPlaySpace.GetCards().Count);
            Assert.AreEqual(newCard, newCardTrain.GetPrevCard());
            Assert.AreEqual(newCardTrain, newCard.GetNextCard());

            // Confirm all cards are moveable
            foreach (Card card in newPlaySpace.GetCards()) {
                Assert.IsTrue(card.isMoveable);
            }
        }

        // Test add card train to play space with single card and only 3 moveable cards
        [Test]
        public void AddsCardTrainToSingleMoveableCard() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();
            newPlaySpace.GetGameManager().SetNumMoveableCards(3);

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newPlaySpace.AddCard(newCard);

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

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
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newPlaySpace.AddCard(newCard);

            // Remove card
            newPlaySpace.RemoveCard(newCard);

            // Confirm card removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
        }

        // Test remove cards from train
        [Test]
        public void RemovesCardsFromTrain() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Remove 2nd card in train
            newPlaySpace.RemoveCard(newCardTrain.GetNextCard());

            // Confirm cards removed
            Assert.AreEqual(newCardTrain, newPlaySpace.GetLastCard());
            Assert.AreEqual(1, newPlaySpace.GetCards().Count);
            Assert.IsNull(newCardTrain.GetNextCard());
        }

        // Test remove all cards from play space
        [Test]
        public void RemovesAllCards() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Remove all cards
            newPlaySpace.RemoveCard(newCardTrain);

            // Confirms all cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
        }

        // Test move cards from one play space to another
        [Test]
        public void MovesCardToNewEmptyPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();
            PlaySpace nextPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Move cards to next play space
            nextPlaySpace.MoveCardToDropSpace(newCardTrain);

            // Confirm cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
            Assert.AreEqual(3, nextPlaySpace.GetCards().Count);
            Assert.IsNotNull(nextPlaySpace.GetLastCard());
        }

        // Test move cards from one play space to another with cards
        [Test]
        public void MovesCardToNewPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();
            PlaySpace nextPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Add card 
            Card newCard = CardTests.CreateTestCard(10, Suit.clubs);
            nextPlaySpace.AddCard(newCard);
            newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            nextPlaySpace.AddCard(newCard);

            // Move cards to next play space
            nextPlaySpace.MoveCardToDropSpace(newCardTrain);

            // Confirm cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
            Assert.AreEqual(5, nextPlaySpace.GetCards().Count);
            Assert.IsNotNull(nextPlaySpace.GetLastCard());
        }

        // Test box collider size update
        [Test]
        public void BoxColliderUpdatesSize() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Confirm initial size
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().sizeDelta.y);

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);

            // Confirm still initial size and placeholder is deactivated
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().rect.height);
            Assert.IsFalse(newPlaySpace.cardPlaceholder.activeSelf);

            // Add second card
            Card nextCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(nextCard);

            // Confirm size updates correctly
            Assert.AreEqual(3, newPlaySpace.gameObject.transform.childCount);
            Assert.AreEqual(87.5, newPlaySpace.GetComponent<RectTransform>().rect.height);

            // Remove both cards
            newPlaySpace.RemoveCard(newCard);

            // Confirm size updates correctly
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().rect.height);

            // Remove both cards
            newPlaySpace.RemoveCard(newCard);

            // Confirm size updates correctly and placeholder activates
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().rect.height);
            Assert.IsTrue(newPlaySpace.cardPlaceholder.activeSelf);
        }
    }
}
