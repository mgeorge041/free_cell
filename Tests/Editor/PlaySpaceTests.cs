using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class PlaySpaceTests
    {
        /* Tests
         * 
         * Add card to empty play space
         *      - Card should be moveable
         *      - Card added to list of cards
         * 
         * Add card train to empty play space
         *      - Cards should be moveable
         *      - Cards added to list of cards
         * 
         * Add card to play space with 1 current card
         *      # 1: 1 moveable card
         *          - Card should be moveable
         *          - Bottom card should not be moveable
         *          - Card added to list of cards
         *      # 2: 2 moveable cards
         *          - Cards should be moveable
         *          - Card added to list of cards
         * 
         * Add card train to play space with 1 current card
         *      # 1: 3 moveable cards
         *          - Card train should be moveable
         *          - Bottom card should not be moveable
         *          - Cards added to list of cards
         *      # 2: 4 moveable cards
         *          - Cards should be moveable
         *          - Cards added to list of cards
         *          
         * Move 1 card from empty play space to another
         *      - Card should be moveable
         *      - 1st space should have no cards in list
         *      - 2nd space should have card in list
         *      
         * Move 1 card from 1 card play space to empty play space
         *      - Card should be moveable
         *      - 1st space should have 1 card in list
         *      - 2nd space should have card in list
         *      
         * Move 1 card from 1 card play space to 1 play space with 1 current card
         *      # 1: 1 moveable card 
         *          - Card should be moveable
         *          - 1st space card should be moveable
         *          - 2nd space bottom card should not be moveable
         *      # 2: 2 moveable cards
         *          - 1st space card should be moveable
         *          - 2nd space cards should be moveable
         * 
         */

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


        // Test add card to play space with single card
        [Test]
        public void AddsCardToSingleCard() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Create cards
            Card bottomCard = CardTests.CreateTestCard(6, Suit.diamonds);
            Card topCard = CardTests.CreateTestCard(5, Suit.clubs);

            // Add cards when 1 moveable card
            newPlaySpace.GetGameManager().SetNumMoveableCards(1);
            newPlaySpace.AddCard(bottomCard);
            newPlaySpace.AddCard(topCard);

            // Confirm bottom card not moveable
            Assert.IsFalse(bottomCard.isMoveable);
            Assert.IsTrue(topCard.isMoveable);
            Assert.AreEqual(2, newPlaySpace.GetCards().Count);

            // Clear play space cards
            newPlaySpace.ClearCards();

            // Add cards when 2 moveable cards
            newPlaySpace.GetGameManager().SetNumMoveableCards(2);
            newPlaySpace.AddCard(bottomCard);
            newPlaySpace.AddCard(topCard);

            // Confirm cards moveable
            Assert.IsTrue(bottomCard.isMoveable);
            Assert.IsTrue(topCard.isMoveable);
            Assert.AreEqual(2, newPlaySpace.GetCards().Count);
        }


        // Test add card train to play space with single card
        [Test]
        public void AddsCardTrainToSingleCard() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add new card
            Card newCard = CardTests.CreateTestCard(6, Suit.diamonds);
            Card newCardTrain = CardTests.CreateTestCardTrain();

            // Add train of cards when 3 moveable cards
            newPlaySpace.GetGameManager().SetNumMoveableCards(3);
            newPlaySpace.AddCard(newCard);
            newPlaySpace.AddCard(newCardTrain);

            // Confirm 3 cards added
            Assert.AreEqual(4, newPlaySpace.GetCards().Count);
            Assert.AreEqual(newCard, newCardTrain.GetPrevCard());
            Assert.AreEqual(newCardTrain, newCard.GetNextCard());

            // Confirm all cards are moveable in train but bottom is not
            Card trainCard = newCardTrain;
            while (trainCard != null) {
                Assert.IsTrue(trainCard.isMoveable);
                trainCard = trainCard.GetNextCard();
            }
            Assert.IsFalse(newCard.isMoveable);

            // Clear cards
            newPlaySpace.ClearCards();

            // Add train of cards when 4 moveable cards
            newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.GetGameManager().SetNumMoveableCards(4);
            newPlaySpace.AddCard(newCard);
            newPlaySpace.AddCard(newCardTrain);

            // Confirm 3 cards added
            Assert.AreEqual(4, newPlaySpace.GetCards().Count);
            Assert.AreEqual(newCard, newCardTrain.GetPrevCard());
            Assert.AreEqual(newCardTrain, newCard.GetNextCard());

            // Confirm all cards are moveable in train but bottom is not
            trainCard = newCard;
            while (trainCard != null) {
                Assert.IsTrue(trainCard.isMoveable);
                trainCard = trainCard.GetNextCard();
            }
        }


        // Test move cards from one play space to another
        [Test]
        public void MovesCardToNewEmptyPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();
            PlaySpace nextPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Add card
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);

            // Move card to next play space
            nextPlaySpace.MoveCardToDropSpace(newCard);

            // Confirm cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
            Assert.AreEqual(1, nextPlaySpace.GetCards().Count);
            Assert.IsTrue(newCard.isMoveable);
        }


        // Test move cards from one play space to another with cards
        [Test]
        public void MovesCardToNewPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();
            PlaySpace nextPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Create cards
            Card firstCard = CardTests.CreateTestCard(5, Suit.diamonds);
            Card secondCard = CardTests.CreateTestCard(6, Suit.clubs);

            // Add cards
            nextPlaySpace.SetGameManager(newPlaySpace.GetGameManager());
            newPlaySpace.GetGameManager().SetNumMoveableCards(1);
            newPlaySpace.AddCard(firstCard);
            nextPlaySpace.AddCard(secondCard);

            // Move card to next play space when 1 moveable card
            nextPlaySpace.MoveCardToDropSpace(firstCard);

            // Confirm cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
            Assert.AreEqual(2, nextPlaySpace.GetCards().Count);
            Assert.IsTrue(firstCard.isMoveable);
            Assert.IsFalse(secondCard.isMoveable);

            // Clear cards
            nextPlaySpace.ClearCards();

            // Add cards
            nextPlaySpace.SetGameManager(newPlaySpace.GetGameManager());
            newPlaySpace.GetGameManager().SetNumMoveableCards(2);
            newPlaySpace.AddCard(firstCard);
            nextPlaySpace.AddCard(secondCard);

            // Move card to next play space when 2 moveable card
            nextPlaySpace.MoveCardToDropSpace(firstCard);

            // Confirm cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
            Assert.AreEqual(2, nextPlaySpace.GetCards().Count);
            Assert.IsTrue(firstCard.isMoveable);
            Assert.IsTrue(secondCard.isMoveable);
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
