using JetBrains.Annotations;
using UnityEngine;

/// <summary>
/// This attribute is used to mark objects that can be picked up by the player.
/// It contains a boolean to check if the player has already picked it up.
/// It implements the Take action, which adds the object to the player's inventory.
/// </summary>
public class Obtainable : Attribute
{
    public bool IsObtained
    {
        get { return inventoryManager.ContainsItem(gameObject); }
    }

    protected override void Awake()
    {
        base.Awake();
        actionName = "Take";
        defaultAnimation = "PickUp";
    }

    /// <summary>
    /// Checks if the action can be executed.
    /// The action can be executed if the item has not been picked up yet.
    /// </summary>
    /// <returns>True if the action can be executed, false otherwise.</returns>
    public override bool IsValid()
    {
        return !IsObtained;
    }

    /// <summary>
    /// Adds the object to the player's inventory.
    /// Displays a message to inform the player that the object has been added to the inventory.
    /// Disables the renderer and collider to make the object invisible and non-interactive.
    /// </summary>
    /// <returns>True if the action was executed successfully, false otherwise.</returns>
    public override bool Execute()
    {
        string displayedName = gameObject.GetComponent<Interactable>().DisplayedName;
        inventoryManager.AddItem(gameObject);
        DisplayMessage($"{displayedName} added to the inventory.");

         // Disable the renderer to make the item invisible
        if (TryGetComponent(out Renderer renderer))
        {
            renderer.enabled = false;
        }

        // Disable the collider to make the item non-interactive
        if (TryGetComponent(out Collider collider))
        {
            collider.enabled = false;
        }
        
        transform.position = player.transform.position;

        return true;
    }
}