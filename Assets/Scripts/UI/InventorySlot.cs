using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// InventorySlot is a script that represents a slot in the player's inventory.
/// </summary>
public class InventorySlot : MonoBehaviour
{
    public GameObject Item;
    private Text slotText;

    void Start()
    {
        slotText = GetComponentInChildren<Text>();
        slotText.text = string.Empty;
    }

    /// <summary>
    /// Sets the item in the inventory slot.
    /// </summary>
    /// <param name="item">The item to set in the inventory slot.</param>
    
    public void SetItem(GameObject item)
    {
        Item = item;
        slotText.text = item.GetComponent<Interactable>().DisplayedName;
    }

    /// <summary>
    /// Clears the inventory slot.
    /// </summary>
    public void Clear()
    {
        Item = null;
        slotText.text = string.Empty;
    }

    /// <summary>
    /// Checks if the inventory slot is empty.
    /// </summary>
    /// <returns>True if the inventory slot is empty, false otherwise.</returns>
    public bool IsEmpty()
    {
        return Item == null;
    }

    /// <summary>
    /// Checks if the inventory slot contains the item.
    /// </summary>
    /// <param name="item">The item to check if it is in the inventory slot.</param>
    /// <returns>True if the inventory slot contains the item, false otherwise.</returns>
    public bool Contains(GameObject item)
    {
        return Item == item;
    }
}
