using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUi : MonoBehaviour
{
    private bool inventoryOpen = false;
    public bool InventoryOpen => inventoryOpen;

    public GameObject InventoryParent;
    public GameObject InventoryTab;
    public GameObject CraftingTab;

    private List<ItemSlot> itemSlotList = new();

    public GameObject inventorySlotPrefab;
    public GameObject craftingSlotPrefab;

    public Transform inventoryItemTransform;
    public Transform craftingItemTransform;

    private void Start()
    {
        Inventory.instance.onItemChange += UpdateInventoryUI;
        UpdateInventoryUI();
        SetUpCraftingRecipes();
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (inventoryOpen)
            {
                //close inv
                CloseInventory();
            }
            else
            {
                //open inv
                OpenInventory();
            }
        }
    }
    private void SetUpCraftingRecipes()
    {
        List<Item> craftingRecipes = GameManager.instance.craftingRecipes;

        foreach (Item recipe in craftingRecipes)
        {
            GameObject Go = Instantiate(craftingSlotPrefab, craftingItemTransform);
            ItemSlot slot = Go.GetComponent<ItemSlot>();
            slot.AddItem(recipe);
        }
    }
    private void UpdateInventoryUI()
    {
        int currentItemCount = Inventory.instance.inventoryItemList.Count;

        if (currentItemCount > itemSlotList.Count)
        {
            AddItemSlots(currentItemCount);
        }
        for (int i = 0; i < itemSlotList.Count; ++i)
        {
            if (i < currentItemCount)
            {
                // update current item in the slot
                itemSlotList[i].AddItem(Inventory.instance.inventoryItemList[i]);
            }
            else
            {
                itemSlotList[i].DestroySlot();
                itemSlotList.RemoveAt(i);
            }
        }
    }

    private void AddItemSlots(int currentItemCount)
    {
        int amount = currentItemCount - itemSlotList.Count;
        for (int i = 0; i < amount; ++i)
        {
            GameObject GO = Instantiate(inventorySlotPrefab, inventoryItemTransform);
            ItemSlot newSlot = GO.GetComponent<ItemSlot>();
            itemSlotList.Add(newSlot);
        }
    }

    private void OpenInventory()
    {
        ChangeCursorState(false);
        inventoryOpen = true;
        InventoryParent.SetActive(true);
        OnInventoryTabClicked();
    }
    private void CloseInventory()
    {
        ChangeCursorState(true);
        inventoryOpen = false;
        InventoryParent.SetActive(false);
    }
    public void OnCraftingTabClicked()
    {
        CraftingTab.SetActive(true);
        InventoryTab.SetActive(false);
    }
    public void OnInventoryTabClicked()
    {
        CraftingTab.SetActive(false);
        InventoryTab.SetActive(true);
    }
    private void ChangeCursorState(bool lockCursor)
    {
        if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
