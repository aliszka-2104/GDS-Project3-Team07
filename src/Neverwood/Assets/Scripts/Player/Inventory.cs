using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public struct Item
{
    public Item(int ID, string name, int maxCount)
    {
        itemID = ID;
        itemName = name;
        itemCount = 0;
        itemMaxCount = maxCount;
    }
    public int itemID;
    public string itemName;
    public int itemCount;
    public int itemMaxCount;
};
public class Inventory : MonoBehaviour
{
    List<Item> items = new List<Item>();

    Item[] existingItems =
    {
        new Item(0, "Flint", 10),
        new Item(1, "Match", 10)
    };

    public Item GetItem(int ID)
    {
        bool gotAlready = false;
        Item itemFound = new Item();
        foreach (Item item in items)
        {
            if (gotAlready = item.itemID == ID)
            {
                itemFound = item;
                break;
            }
        }
        if(!gotAlready)
        {
            throw new System.Exception("Item + ID[" + ID + "] not found");
        }
        return itemFound;
    }

    public bool TryGetItem(int ID)
    {
        bool gotAlready = false;
        foreach (Item item in items)
        {
            gotAlready = item.itemID == ID;
        }
        return gotAlready;
    }

    public void AddItem(int ID)
    {
        Item itemFound = new Item();
        if (TryGetItem(ID))
        {
            itemFound = GetItem(ID);
            if (itemFound.itemCount == itemFound.itemMaxCount)
            {
                throw new System.Exception("Maximum number of + [" + itemFound.itemName + "] item already aquired");
            }
            else
            {
                itemFound.itemCount++;
            }
        }
        else
        {
            int index = 0;
            while(existingItems[index].itemID != ID) { index++; }
            itemFound.itemID = ID;
            itemFound.itemName = existingItems[index].itemName;
            itemFound.itemCount = 1;
            itemFound.itemMaxCount = existingItems[index].itemMaxCount;
            items.Add(itemFound);
        }
    }

    public void RemoveItem(int ID)
    {
        Item itemFound = new Item();
        if(TryGetItem(ID))
        {
            itemFound = GetItem(ID);
            itemFound.itemCount--;
            if(itemFound.itemCount == 0)
            {
                items.Remove(itemFound);
            }
        }
        else
        {
            throw new System.Exception("Item + ID[" + ID + "] not found");
        }
    }
}
