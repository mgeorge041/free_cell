using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;
using UnityEditor;

public abstract class DropSpace : MonoBehaviour
{
    public GameManager gameManager;
    public BoxCollider2D boxCollider;

    // Create generic drop space
    public static T CreateDropSpace<T>() {
        Object dropSpacePrefab;
        if (typeof(T) == typeof(PlaySpace)) {
            dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Play Space.prefab", typeof(GameObject));
        }
        else if (typeof(T) == typeof(FreeSpace)) {
            dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Free Space.prefab", typeof(GameObject));
        }
        else if (typeof(T) == typeof(SuitSpace)) {
            dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Suit Space.prefab", typeof(GameObject));
        }
        else {
            dropSpacePrefab = AssetDatabase.LoadAssetAtPath("Assets/Prefabs/Drop Space.prefab", typeof(GameObject));
        }
        GameObject newDropSpaceObject = (GameObject)Instantiate(dropSpacePrefab);
        T newDropSpace = newDropSpaceObject.GetComponent<T>();

        return newDropSpace;
    }

    // Initialize
    public void Initialize(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Get game manager
    public GameManager GetGameManager() {
        return gameManager;
    }

    // Set game manager
    public void SetGameManager(GameManager gameManager) {
        this.gameManager = gameManager;
    }

    // Determine whether point is in box collider
    public bool Contains(Vector2 point) {
        if (boxCollider.OverlapPoint(point)) {
            return true;
        }
        return false;
    }

    // Add card to drop space
    public abstract void AddCard(Card card);

    // Remove card from drop space
    public abstract void RemoveCard(Card card);

    // Get last card from drop space
    public abstract Card GetLastCard();

    // Move card to drop space
    public bool MoveCardToDropSpace(Card card) {
        if (CanMoveToDropSpace(card)) {
            card.RemoveFromParentDropSpace();
            AddCard(card);
            return true;
        }
        else {
            return false;
        }
    }

    // Determine whether can move to drop space
    public abstract bool CanMoveToDropSpace(Card card);
}
