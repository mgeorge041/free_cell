using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;

namespace Tests
{
    public class GameManagerTests
    {
        // Create test game manager
        public static GameManager CreateTestGameManager() {
            Object gameManagerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Game Manager.prefab", typeof(GameObject));
            GameObject gameManagerObject = (GameObject)Object.Instantiate(gameManagerPrefab);
            GameManager newGameManager = gameManagerObject.GetComponent<GameManager>();

            return newGameManager;
        }

        // Create initialized test game manager
        public static GameManager CreateInitializedTestGameManager() {
            GameManager newGameManager = CreateTestGameManager();
            newGameManager.Initialize();

            return newGameManager;
        }

        // Test creates game manager
        [Test]
        public void CreatesGameManager() {
            GameManager gameManager = CreateTestGameManager();
            Assert.IsNotNull(gameManager);

        }

        // Test creates cards
        [Test]
        public void CreatesDeck() {
            GameManager gameManager = CreateTestGameManager();
            gameManager.CreateCards();

            // Confirm deck is made
            Assert.AreEqual(52, gameManager.GetCards().Count);
        }

        // Test shuffles cards
        [Test]
        public void ShufflesCards() {
            GameManager gameManager = CreateTestGameManager();
            gameManager.CreateCards();

            // Shuffle cards
            Card firstCard = gameManager.GetCards()[0];
            int numCards = gameManager.GetCards().Count;
            gameManager.ShuffleCards();
            Card shuffledFirstCard = gameManager.GetCards()[0];
            int numShuffledCards = gameManager.GetCards().Count;

            // Confirm first card has changed (most likely)
            Assert.AreNotEqual(firstCard, shuffledFirstCard);

            // Confirm number of cards still equal
            Assert.AreEqual(numCards, numShuffledCards);
        }

        // Test creates drop spaces
        [Test]
        public void CreatesDropSpaces() {
            GameManager newGameManager = CreateTestGameManager();
            newGameManager.CreateDropSpaces();

            // Confirm correct number of drop spaces created
            Assert.AreEqual(8, newGameManager.GetPlaySpaces().Length);
            Assert.AreEqual(4, newGameManager.GetFreeSpaces().Length);
            Assert.AreEqual(4, newGameManager.GetSuitSpaces().Length);
        }

        // Test deals cards
        [Test]
        public void DealsCards() {
            GameManager newGameManager = CreateTestGameManager();
            newGameManager.CreateCards();
            newGameManager.CreateDropSpaces();
            newGameManager.DealCards();

            // Confirm correct number of cards in columns
            int numPlaySpaceCards = newGameManager.playSpaces[0].GetNumCards();
            Assert.AreEqual(7, numPlaySpaceCards);
            numPlaySpaceCards = newGameManager.playSpaces[4].GetNumCards();
            Assert.AreEqual(6, numPlaySpaceCards);
        }

        // Test move cards
        [Test]
        public void MovesCards() {
            GameManager newGameManager = CreateInitializedTestGameManager();
            PlaySpace playSpace1 = newGameManager.playSpaces[0];
            PlaySpace playSpace2 = newGameManager.playSpaces[1];
            PlaySpace playSpace3 = newGameManager.playSpaces[2];

            // Create cards
            Card card1 = CardTests.CreateTestCard(5, Suit.clubs);
            Card card2 = CardTests.CreateTestCard(6, Suit.diamonds);
            Card card3 = CardTests.CreateTestCard(10, Suit.clubs);
            card1.SetGameManager(newGameManager);
            card2.SetGameManager(newGameManager);
            card3.SetGameManager(newGameManager);

            // Add cards
            playSpace1.AddCard(card1);
            playSpace2.AddCard(card2);
            playSpace3.AddCard(card3);

            // Move card 1 to play space 2
            newGameManager.MoveCard(card1, playSpace2);

            // Confirm card has been moved
            Assert.AreNotEqual(card1, playSpace1.GetLastCard());
            Assert.AreEqual(card1, playSpace2.GetLastCard());

            // Move card 3 to play space 2
            newGameManager.MoveCard(card3, playSpace2);

            // Confirm card has not been moved
            Assert.AreEqual(card3, playSpace3.GetLastCard());
            Assert.AreNotEqual(card3, playSpace2.GetLastCard());
        }
    }
}
