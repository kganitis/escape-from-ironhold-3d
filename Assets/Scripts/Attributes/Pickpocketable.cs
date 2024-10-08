using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This attribute is used for interactables that can be pickpocketed by the player.
/// The pickpocketable item is an obtainable that is not visible to the player until it is stolen.
/// </summary>
/// <remarks>It is used to mark NPCs that have items that can be stolen by the player.</remarks>
public class Pickpocketable : Attribute
{
    private GameObject owner;
    public Obtainable item;
    public bool IsPickpocketed
    {
        get { return item != null && item.IsObtained; }
    }

    protected override void Awake()
    {
        base.Awake();
        actionName = "Pickpocket";
        defaultAnimation = "Pickpocket";
        owner = gameObject;
    }

    /// <summary>
    /// The action is valid if the item has not been pickpocketed yet.
    /// </summary>
    /// <returns>True if the action is valid, false otherwise.</returns>
    public override bool IsValid()
    {
        return !IsPickpocketed;
    }

    /// <summary>
    /// Executes the pickpocket action of the interactable.
    /// The item is stolen from the owner and added to the player's inventory.
    /// It displays a message to the player indicating that the item has been stolen.
    /// </summary>
    /// <returns>True if the action was successful, false otherwise.</returns>
    public override bool Execute()
    {
        string itemName = item.GetComponent<Interactable>().DisplayedName;
        string ownerName = owner.GetComponent<Interactable>().DisplayedName;

        Obtainable obtainAction = this.item;
        obtainAction.Silent = true;
        if (!obtainAction.Execute())
            return false;

        string pickpocketMessage = $"You stole the <color={itemTextColor}>{itemName}</color> from the <color={itemTextColor}>{ownerName}</color>!";
        DisplayMessage(pickpocketMessage);

        this.Silent = true;
        return base.Execute();
    }
}
