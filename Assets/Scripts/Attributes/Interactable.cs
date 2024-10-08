using UnityEngine;

/// <summary>
/// This attribute is used to mark objects that can be interacted with by the player.
/// It contains a name, a description, and a boolean to check if the player has already interacted with it.
/// It implements the Examine action, which displays the object's description.
/// </summary>
public class Interactable : Attribute
{
    [SerializeField] private string displayedName = "No name";
    [SerializeField] private string description = "No description";
    [SerializeField] private bool discovered = false;

    public virtual string DisplayedName 
    { 
        get { return displayedName; } 
        set { displayedName = value; } 
    }

    public virtual string ColoredName
    {
        get { return $"<color={itemTextColor}>{DisplayedName}</color>"; }
    }

    public virtual string Description 
    { 
        get { return description; } 
        set { description = value; } 
    }

    public virtual bool Discovered 
    { 
        get { return discovered; } 
        set { discovered = value; } 
    }

    protected override void Awake()
    {
        base.Awake();
        actionName = "Examine";
        defaultAnimation = "SearchLow";
    }

    public override bool Execute()
    {
        Examine();
        return base.Execute();
    }

    /// <summary>
    /// Displays the object's description.
    /// If the object has not been discovered yet, it will be discovered.
    /// </summary>
    /// <remarks>It also turns off the silent mode, in case it was turned on by another action
    /// that didn't want to display the object's description.</remarks>
    private void Examine()
    {
        DisplayMessage(Description);
        Silent = false;
        Discovered = true;
    }
}
