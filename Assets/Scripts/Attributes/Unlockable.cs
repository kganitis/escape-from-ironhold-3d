using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This attribute is used to mark objects that can be unlocked by the player.
/// It contains a boolean to check if the object is locked.
/// It contains a reference to the key required to unlock the object.
/// It contains a boolean to check if the object can be unlocked with a lockpick.
/// It implements the Unlock action, which unlocks the object if the player has the required key or lockpick.
/// </summary>
public class Unlockable : Attribute
{
    [SerializeField] private bool isLocked;
    public GameObject Key; // The key required to unlock the object
    public bool CanBePicked; // Whether the object can be unlocked with a lockpick

    public bool IsLocked
    {
        get { return isLocked; }
        set { isLocked = value; }
    }

    protected override void Awake()
    {
        base.Awake();
        actionName = "Unlock";
        defaultAnimation = "ButtonPush";
    }

    /// <summary>
    /// Τhe action can be executed if the object is locked.
    /// </summary>
    public override bool IsValid()
    {
        return isLocked;
    }

    /// <summary>
    /// Unlocks the object if the player has the required key or lockpick.
    /// Displays a message to inform the player that the object has been unlocked.
    /// If the player doesn't have the required key or lockpick, displays a message to inform the player that they're missing the required item.
    /// </summary>
    public override bool Execute()
    {
        GameObject tool;
        GameObject lockpick = GameObject.Find("IO_Prop_Lockpick_01");

        if (Key != null && inventoryManager.ContainsItem(Key))
        {
            tool = Key;
        }
        else if (CanBePicked && inventoryManager.ContainsItem(lockpick))
        {
            tool = lockpick;
        }
        else
        {
            string unlock_fail_message;
            if (CanBePicked && Key != null)
            {
                unlock_fail_message = $"You need a {Key.GetComponent<Interactable>().ColoredName} or a {lockpick.GetComponent<Interactable>().ColoredName} to unlock this.";
            }
            else if (!CanBePicked && Key != null)
            {
                unlock_fail_message = $"You need a {Key.GetComponent<Interactable>().ColoredName} to unlock this.";
            }
            else if (CanBePicked && Key == null)
            {
                unlock_fail_message = $"You need a {lockpick.GetComponent<Interactable>().ColoredName} to unlock this.";
            }
            else
            {
                unlock_fail_message = $"There isn't a way to unlock this.";
            }
            DisplayMessage(unlock_fail_message);
            return false;
        }

        IsLocked = false;

        string coloredName = GetComponent<Interactable>().ColoredName;
        DisplayMessage($"{coloredName} unlocked successfully using the {tool.GetComponent<Interactable>().ColoredName}.");

        return base.Execute();
    }
}