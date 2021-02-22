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
            Object gameManagerPrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/GameManager.prefab", typeof(GameObject));
            GameObject gameManagerObject = (GameObject)Object.Instantiate(gameManagerPrefab);
            GameManager newGameManager = gameManagerObject.GetComponent<GameManager>();

            return newGameManager;
        }

        // Test creates game manager
        [Test]
        public void CreatesGameManager()
        {
            GameManager gameManager = CreateTestGameManager();
            Assert.IsNotNull(gameManager);
        }
    }
}
