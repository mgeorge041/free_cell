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
            newGameManager.Initialize();

            return newGameManager;
        }

        // Test creates game manager
        [Test]
        public void CreatesGameManager() { 
            GameManager gameManager = CreateTestGameManager();
            Assert.IsNotNull(gameManager);
            Assert.AreEqual(8, gameManager.GetPlaySpaces().Length);
            Assert.AreEqual(4, gameManager.GetFreeSpaces().Length);
            Assert.AreEqual(4, gameManager.GetSuitSpaces().Length);
        }

        // Test creates cards
        [Test]
        public void CreatesDeck() {
            GameManager gameManager = CreateTestGameManager();
            gameManager.CreateCards();

            // Confirm deck is made
            Assert.AreEqual(52, gameManager.GetCards().Count);
        }
    }
}
