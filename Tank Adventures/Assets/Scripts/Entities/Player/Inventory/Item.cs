using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Instances of items
/// </summary>
[Serializable]
public class Item
{
    public ItemData data;
    public int quantity;

    public Item(ItemData value)
    {
        data = value;
        quantity = 1;
    }
}
