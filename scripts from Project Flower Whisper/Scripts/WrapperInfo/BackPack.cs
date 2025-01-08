using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Backpack : MonoBehaviour
{
    #region Singleton
    public static Backpack instance;

    void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("More than one instance of Backpack found!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    #endregion

    private Queue<Item> itemsQueue = new Queue<Item>();
    public List<Item> itemsList = new List<Item>(); // 仅用于Inspector显示
    public int capacity = 3;

    public bool Add(Item item)
    {
        if (itemsQueue.Count >= capacity)
        {
            Item removedItem = itemsQueue.Dequeue();
            itemsList.Remove(removedItem);
            Debug.Log("Removed oldest item: " + removedItem.name);
        }
        itemsQueue.Enqueue(item);
        itemsList.Add(item);
        Debug.Log("Added item to backpack: " + item.name);
        RefreshInspectorList();
        return true;
    }

    public void TransferToColorOwned()
    {
        while (itemsQueue.Count > 0)
        {
            Item item = itemsQueue.Dequeue();
            itemsList.Remove(item);
            ColorOwned.instance.Add(item);
            Debug.Log("Transferred item to ColorOwned: " + item.name);
        }
        RefreshInspectorList();
    }

    public Item[] GetItemsArray()
    {
        return itemsQueue.ToArray();
    }

    // 强制刷新Inspector中的列表显示
    private void RefreshInspectorList()
    {
        
        var tempList = new List<Item>(itemsList);
        itemsList.Clear();
        itemsList.AddRange(tempList);
    }
}
