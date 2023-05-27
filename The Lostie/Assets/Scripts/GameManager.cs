using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    #region singleton
    public static GameManager instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }
    #endregion
    public List<Item> itemList = new();
    public List<Item> craftingRecipes = new();

    public Transform canvas;
    public GameObject itemInfoPrefab;
    private GameObject currentItemInfo = null;
    public Transform mainCanvas;
    public Transform hotBarTransform;
    public Transform inventoryTransform;

    public float moveDescX = 0f;
    public float moveDescY = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.X))
        {
            Item newItem = itemList[Random.Range(0, itemList.Count)];

            Inventory.instance.AddItem(Instantiate(newItem));
        }
    }

    public void OnStatItemuUse(StatItemType itemType, int amount)
    {
        Debug.Log("Consuming " + itemType + " Add amount " + amount);
    }

    public void DisplayItemInfo(string itemName, string itemDescription, Vector2 buttonPos)
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }

        buttonPos.x += moveDescX;
        buttonPos.y += moveDescY;

        currentItemInfo = Instantiate(itemInfoPrefab, buttonPos, Quaternion.identity, canvas);
        currentItemInfo.GetComponent<ItemInfo>().SetUp(itemName, itemDescription);
    }
    public void DestroyItemInfo()
    {
        if (currentItemInfo != null)
        {
            Destroy(currentItemInfo.gameObject);
        }
    }
}
