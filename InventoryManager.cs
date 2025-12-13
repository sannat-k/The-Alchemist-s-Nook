using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    // List to hold the ingredients in the inventory
    public List<ItemData> ingredients = new List<ItemData>();

    // Event triggered when the inventory is updated
    public static event System.Action OnInventoryUpdated;

    public bool AddItem(ItemData item)
    {
        if (item == null)
            return false;
        ingredients.Add(item);
        Debug.Log("Picked up: " + item.itemName);

        // Trigger the inventory updated event
        if (OnInventoryUpdated != null)
        {
            OnInventoryUpdated.Invoke();
        }
        return true;
    }

    public void RemoveItem(ItemData item)
    {
        if (ingredients.Contains(item))
        {
            ingredients.Remove(item);
            Debug.Log("Removed: " + item.itemName);

            // Trigger the inventory updated event 
            if (OnInventoryUpdated != null)
            {
                OnInventoryUpdated.Invoke();
            }
        }
    }
}
