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
            PlaySpace newPlaySpace = CreateTestPlaySpace();

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);

            // Confirm new card is added
            Assert.AreEqual(newCard, newPlaySpace.GetLastCard());
            Assert.IsNull(newCard.GetPrevCard());
            Assert.IsNull(newCard.GetNextCard());
        }


        // Test add 2 cards
        [Test]
        public void AddsCards() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpace();

            // Add new cards
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);
            Card nextCard = CardTests.CreateTestCard(6, Suit.diamonds);
            newPlaySpace.AddCard(nextCard);

            // Confirm new cards are added
            Assert.AreEqual(nextCard, newPlaySpace.GetLastCard());
            Assert.AreEqual(2, newPlaySpace.GetCards().Count);

            // Confirm next prev cards set correctly
            Assert.IsNull(newCard.GetPrevCard());
            Assert.AreEqual(nextCard, newCard.GetNextCard());
            Assert.AreEqual(newCard, nextCard.GetPrevCard());
            Assert.IsNull(nextCard.GetNextCard());
        }


        // Test add card train
        [Test]
        public void AddsCardTrain() {
            // Create play space
            PlaySpace newPlaySpace = CreateTestPlaySpace();

            // Add train of cards
            Card newCard = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCard);

            // Confirm 3 cards added
            Assert.AreEqual(3, newPlaySpace.GetCards().Count);
        }


        // Test remove card
        [Test]
        public void RemovesCard() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();

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
            PlaySpace newPlaySpace = CreateTestPlaySpace();

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
        public void RemovesAllCardsFromTrain() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();

            // Add train of cards
            Card newCardTrain = CardTests.CreateTestCardTrain();
            newPlaySpace.AddCard(newCardTrain);

            // Remove all cards
            newPlaySpace.RemoveCard(newCardTrain);

            // Confirms all cards removed
            Assert.IsNull(newPlaySpace.GetLastCard());
            Assert.AreEqual(0, newPlaySpace.GetCards().Count);
        }


        // Test move to empty space
        [Test]
        public void CanMoveToEmptyPlaySpace() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();
            Card newCard = CardTests.CreateTestCard();

            // Confirm card can move to empty play space
            bool canMove = newPlaySpace.CanMoveToDropSpace(newCard);
            Assert.IsTrue(canMove);
        }


        // Test move to space with current card
        [Test]
        public void CanMoveToPlaySpaceWithCard() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();
            newPlaySpace.AddCard(newCard);

            // Confirm next card 1 lower different suit correct
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newPlaySpace.CanMoveToDropSpace(nextCard);
            Assert.IsTrue(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newPlaySpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newPlaySpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newPlaySpace.CanMoveToDropSpace(nextCard);
            Assert.IsFalse(canMove);
        }


        // Test cards in order
        [Test]
        public void CardsInOrder() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();
            Card newCard = CardTests.CreateTestCard();
            int cardValue = newCard.GetCardValue();

            // Confirm next card 1 lower different suit correct
            Card nextCard = CardTests.CreateTestCard(cardValue - 1, Suit.diamonds);
            bool canMove = newPlaySpace.CardsInOrder(newCard, nextCard);
            Assert.IsTrue(canMove);

            // Confirm next card 1 higher different suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, Suit.diamonds);
            canMove = newPlaySpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 lower same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue - 1, newCard.GetCardSuit());
            canMove = newPlaySpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);

            // Confirm next card 1 higher same suit incorrect
            nextCard = CardTests.CreateTestCard(cardValue + 1, newCard.GetCardSuit());
            canMove = newPlaySpace.CardsInOrder(newCard, nextCard);
            Assert.IsFalse(canMove);
        }


        // Test card train moveability
        [Test]
        public void SetsCardTrainMoveability() {
            PlaySpace newPlaySpace = CreateTestPlaySpace();
            Card newCard = CardTests.CreateTestCard(5, Suit.clubs);
            Card secondCard = CardTests.CreateTestCard(4, Suit.diamonds);

            // Add cards
            newPlaySpace.AddCard(newCard);
            newPlaySpace.AddCard(secondCard);

            // Confirm both cards can move
            newPlaySpace.SetTrainMoveability(2);
            Assert.IsTrue(newCard.isMoveable);
            Assert.IsTrue(secondCard.isMoveable);

            // Confirm bottom card can move
            newPlaySpace.SetTrainMoveability(1);
            Assert.IsFalse(newCard.isMoveable);
            Assert.IsTrue(secondCard.isMoveable);
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


        // Test box collider size update
        [Test]
        public void BoxColliderUpdatesSize() {
            PlaySpace newPlaySpace = CreateTestPlaySpaceWithGameManager();

            // Confirm initial size
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().sizeDelta.y);

            // Add new card
            Card newCard = CardTests.CreateTestCard();
            newPlaySpace.AddCard(newCard);

            // Confirm still initial size
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().rect.height);

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

            // Confirm size updates correctly
            Assert.AreEqual(70, newPlaySpace.GetComponent<RectTransform>().rect.height);
        }
    }
}
