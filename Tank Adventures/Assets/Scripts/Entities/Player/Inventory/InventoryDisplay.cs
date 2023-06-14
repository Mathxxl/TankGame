using Entities.Player.Inventory;
using UnityEngine;

/// <summary>
/// Manages the display of the inventory
/// </summary>
public class InventoryDisplay : MonoBehaviour
{
    #region Attributes

    [SerializeField] private Inventory inventory;
    [SerializeField] private ItemDisplay[] slots;

    #endregion

    #region Methods

    private void Start()
    {
        UpdateInventory();
    }

    private void UpdateInventory()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (i < inventory.items.Count)
            {
                slots[i].gameObject.SetActive(true);
                slots[i].UpdateItemDisplay(inventory.items[i].data);
            }
            else
            {
                slots[i].gameObject.SetActive(false);
            }
        }
    }

    public void DropItem(int index)
    {
        //Create new object
        GameObject droppedItem = new GameObject();
        droppedItem.AddComponent<Rigidbody>();
        droppedItem.AddComponent<ItemContainer>().item = inventory.items[index];
        GameObject model = Instantiate(inventory.items[index].data.model, droppedItem.transform);
        //Removes item from inventory
        inventory.items.RemoveAt(index);
        //Update inventory
        UpdateInventory();
    }

    #endregion
}