using UnityEngine;

/// <summary>
/// This attribute is used to mark objects that can be opened or closed by the player.
/// It contains a boolean to check if the object is open.
/// It also contains methods to open and close the object.
/// </summary>
public class Openable : Attribute
{
    public bool IsOpen;
    
    public override string ActionName => IsOpen ? "Close": "Open";

    protected override void Awake()
    {
        base.Awake();
        actionName = "Open";
        defaultAnimation = "Open";
    }

    public override bool Execute()
    {
        if (IsOpen)
            Close();
        else
            Open();

        return base.Execute();
    }

    public virtual void Open()
    {
        IsOpen = true;
    }

    public virtual void Close()
    {
        IsOpen = false;
    }
}