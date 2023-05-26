using UnityEngine;

/// <summary>
/// Item container for pick up purposes
/// </summary>
public class ItemContainer : MonoBehaviour
{
        public Item item;

        public Item TakeItem()
        {
                Destroy(gameObject);
                return item;
        }
}
