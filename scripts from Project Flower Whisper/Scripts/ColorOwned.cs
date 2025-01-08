using System.Collections.Generic;
using UnityEngine;

public class ColorOwned : MonoBehaviour
{
    #region Singleton
    // Singleton pattern to ensure only one instance of ColorOwned exists
    public static ColorOwned instance;

    void Awake()
    {
        // Check if an instance of ColorOwned already exists
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of ColorOwned found");
            return;
        }
        // Assign the instance to this script
        instance = this;
    }
    #endregion

    // Delegate for notifying when items change in the inventory
    public delegate void OnItemChanged();
    public OnItemChanged onItemChangedCallBack;

    // List to store the items and their quantities in the inventory
    public List<ItemStack> items = new List<ItemStack>();

    // Maximum number of different items the inventory can hold
    public int space = 12;

    // Maximum total quantity of items (across all stacks) the inventory can hold
    public int maxTotalQuantity = 12;

    // Current total quantity of items in the inventory
    private int totalQuantity = 0;

    // Method to add an item to the inventory
    public bool Add(Item item)
    {
        // Ensure the item is not a default item
        if (!item.isDefaultItem)
        {
            // Check if the item already exists in the inventory
            ItemStack existingStack = items.Find(stack => stack.item.name == item.name);

            if (existingStack != null)
            {
                // If the total quantity exceeds the maximum allowed, prevent adding more items
                if (totalQuantity >= maxTotalQuantity)
                {
                    Debug.Log("Not enough room for more pigment");
                    return false;
                }
                // Increase the quantity of the existing item stack
                existingStack.quantity++;
                totalQuantity++;
            }
            else
            {
                // If inventory is full or total quantity exceeds the maximum, prevent adding new items
                if (items.Count >= space || totalQuantity >= maxTotalQuantity)
                {
                    Debug.Log("Not enough room");
                    return false;
                }
                // Add a new item stack to the inventory
                items.Add(new ItemStack(item, 1));
                totalQuantity++;
            }

            // Invoke the callback to notify listeners that the inventory has changed
            if (onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            }
        }
        return true;
    }

    // Method to remove an item from the inventory
    public void Remove(Item item)
    {
        // Find the item stack in the inventory
        ItemStack existingStack = items.Find(stack => stack.item.name == item.name);

        if (existingStack != null)
        {
            // Decrease the quantity of the item stack
            existingStack.quantity--;
            totalQuantity--;

            // If the quantity of the item stack reaches zero, remove the stack from the inventory
            if (existingStack.quantity <= 0)
            {
                items.Remove(existingStack);
            }

            // Invoke the callback to notify listeners that the inventory has changed
            if (onItemChangedCallBack != null)
            {
                onItemChangedCallBack.Invoke();
            }
        }
    }

    // Method to retrieve the list of items in the inventory
    public List<ItemStack> GetItems()
    {
        return items;
    }

    // Method to get the current total quantity of items in the inventory
    public int GetTotalQuantity()
    {
        return totalQuantity;
    }
}

// Class representing a stack of items, containing the item and its quantity
public class ItemStack
{
    public Item item; // The item in the stack
    public int quantity; // The quantity of the item in the stack

    // Constructor to initialize an item stack with the item and its quantity
    public ItemStack(Item item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}
