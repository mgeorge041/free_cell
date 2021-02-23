using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class DropSpaceTests
    {
        // Create test drop space
        public static DropSpace CreateTestDropSpace() {
            DropSpace newDropSpace = DropSpace.CreateDropSpace<DropSpace>();
            return newDropSpace;
        }

        // Create test drop space with game manager
        public static DropSpace CreateTestDropSpaceWithGameManager() {
            DropSpace newDropSpace = DropSpace.CreateDropSpace<DropSpace>();

            GameManager gameManager = GameManagerTests.CreateTestGameManager();
            newDropSpace.SetGameManager(gameManager);

            return newDropSpace;
        }

        // Test create drop space
        [Test]
        public void CreatesDropSpace() {
            DropSpace newDropSpace = DropSpace.CreateDropSpace<DropSpace>();
            Assert.IsNotNull(newDropSpace);
        }

        // Test create drop space with game manager
        [Test]
        public void CreatesDropSpaceWithGameManager() {
            DropSpace newDropSpace = CreateTestDropSpaceWithGameManager();
            Assert.IsNotNull(newDropSpace);
            Assert.IsNotNull(newDropSpace.GetGameManager());
        }
    }
}
