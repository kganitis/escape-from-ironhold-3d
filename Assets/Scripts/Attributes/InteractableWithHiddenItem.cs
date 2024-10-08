using UnityEngine;

/// <summary>
/// This attribute is used for interactables that have a hidden item that can be found by the player.
/// The hidden item is an obtainable that is not visible to the player until it is found.
/// </summary>
public class InteractableWithHiddenItem : Interactable
{
    public Obtainable hiddenItem;

    /// <summary>
    /// Executes the obtain action of the interactable.
    /// If the hidden item has not been discovered, it will be revealed to the player.
    /// </summary>
    /// <returns>True if the action was executed successfully, false otherwise.</returns>
    public override bool Execute()
    {
        if (Discovered)
        {
            return base.Execute();
        }

        hiddenItem.Silent = true;
        if (!hiddenItem.Execute())
            return false;

        string itemName = hiddenItem.GetComponent<Interactable>().DisplayedName;
        string itemFoundMessage = $"{Description}\nYou found a hidden <color={itemTextColor}>{itemName}</color> in the <color={itemTextColor}>{DisplayedName}</color>!";
        DisplayMessage(itemFoundMessage);

        Silent = true;
        return base.Execute();
    }
}
