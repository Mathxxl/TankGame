using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Inventory main class
/// </summary>
[CreateAssetMenu]
public class Inventory : ScriptableObject
{
    #region Attributes
    
    public List<Item> items = new();
    public int maxItems = 10;
    
    #endregion

    #region Methods

    public bool AddItem(Item item)
    {
        //Look for an empty slot
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i] == null)
            {
                items[i] = item;
                return true;
            }
        }

        //Add a new item if the inventory has space
        if (items.Count < maxItems)
        {
            items.Add(item);
            return true;
        }

        //No space
        return false;
    }

    #endregion

}
