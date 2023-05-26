using System;
using UnityEngine;

/// <summary>
/// Manages the player's inventory 
/// </summary>
public class PlayerInventory : MonoBehaviour
{
    public Inventory inventory;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ItemContainer foundItem))
        {
            inventory.AddItem(foundItem.TakeItem());
        }
    }
}
