using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventoryManager is a script that manages the player's inventory.
/// It is used to add and remove items from the inventory.
/// </summary>
public class InventoryManager : MonoBehaviour
{
    public GameObject inventoryPanel;
    public GameObject inventorySlotPrefab;
    public int slotCount = 6;
    private List<GameObject> slots;

    void Start()
    {
        CreateSlots();
    }

    /// <summary>
    /// Creates the inventory slots based on the slot count.
    /// </summary>
    private void CreateSlots()
    {
        slots = new List<GameObject>();

        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(inventorySlotPrefab, inventoryPanel.transform);
            slots.Add(slot);
        }
    }

    /// <summary>
    /// It adds the item to the first available slot in the inventory.
    /// </summary>
    /// <param name="item">The item to add to the inventory.</param>
    public void AddItem(GameObject item)
    {
        foreach (GameObject slot in slots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot.IsEmpty())
            {
                inventorySlot.SetItem(item);
                break;
            }
        }
    }

    /// <summary>
    /// Removes the item from the inventory.
    /// </summary>
    /// <param name="item">The item to remove from the inventory.</param>
    public void RemoveItem(GameObject item)
    {
        foreach (GameObject slot in slots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot.Contains(item))
            {
                inventorySlot.Clear();
                break;
            }
        }
    }

    /// <summary>
    /// Checks if the inventory contains the item.
    /// </summary>
    /// <param name="item">The item to check if it is in the inventory.</param>
    /// <returns>True if the item is in the inventory, false otherwise.</returns>
    public bool ContainsItem(GameObject item)
    {
        foreach (GameObject slot in slots)
        {
            InventorySlot inventorySlot = slot.GetComponent<InventorySlot>();
            if (inventorySlot.Contains(item))
            {
                return true;
            }
        }
        return false;
    }
}
