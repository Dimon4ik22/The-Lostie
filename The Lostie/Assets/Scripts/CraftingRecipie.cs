using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(fileName = "Item", menuName = "CraftingRecipe/baseRecipe")]
public class CraftingRecipie : Item
{
    public Item result;
    public Ingredient[] ingredients;

    private bool CanCraft()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            bool containsCurrentIngredient = Inventory.instance.ContainsItem(ingredient.item, ingredient.amount);

            if (!containsCurrentIngredient)
            {
                return false;
            }
        }

        return true;
    }

    private void RemoveIngredientsFromIventory()
    {
        foreach (Ingredient ingredient in ingredients)
        {
            Inventory.instance.RemoveItems(ingredient.item, ingredient.amount);
        }
    }

    public override void Use()
    {
        if (CanCraft())
        {
            //remove items
            RemoveIngredientsFromIventory();

            //add a item to the inventory
            Inventory.instance.AddItem(result);
            Debug.Log("You just crafted a: " + result.name);
        }
        else
        {
            Debug.Log("You dont have enaugh ingredients to craft: " + result.name);
        }
    }
    public override string GetItemDescription()
    {
        string itemIngridients = "";
        foreach (Ingredient ingredient in ingredients)
        {
            itemIngridients += "- " + ingredient.amount + " " + ingredient.item.name + "\n";
        }
        return itemIngridients;
    }

    [System.Serializable]
    public class Ingredient
    {
        public Item item;
        public int amount;
    }
}