using UnityEngine;

/// <summary>
/// This attribute is used to mark objects that the player can enter into.
/// It contains a boolean to check if the object is empty.
/// It implements the Get in and Get out actions, which move the player inside or outside the object.
/// </summary>
public class Enterable : Attribute
{
    [HideInInspector] public bool IsEmpty;
    private Vector3 enteredPosition; // The position where the player entered the object

    public override string ActionName => IsEmpty ? "Get in": "Get out";

    protected override void Awake()
    {
        base.Awake();
        IsEmpty = true;
        actionName = "Get in";
        defaultAnimation = "Idle";
    }

    /// <summary>
    /// Executes the Get in or Get out action depending on the state of the object.
    /// If the object is empty, the player will get in.
    /// If the object is not empty, the player will get out.
    /// </summary>
    /// <returns>True if the action was executed successfully, false otherwise.</returns>
    public override bool Execute()
    {
        if (IsEmpty)
        {
            GetIn();
        }
        else
        {
            GetOut();
        }
        return base.Execute();
    }

    /// <summary>
    /// Moves the player inside the object.
    /// Disables the player's renderer to make it invisible.
    /// Pauses the player's agent to prevent it from moving.
    /// Changes the player's layer to Undetectable to prevent it from being detected by enemies.
    /// Saves the position where the player entered the object.
    /// </summary>
    public void GetIn()
    {
        IsEmpty = false;
        enteredPosition = gameObject.transform.position;
        player.transform.position = gameObject.transform.position;
        player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        playerMovement.PauseAgent();
        playerMovement.active = false;
        playerInteraction.enteredObject = gameObject;
        player.layer = LayerMask.NameToLayer("Undetectable");
    }

    /// <summary>
    /// Moves the player outside the object.
    /// Enables the player's renderer to make it visible.
    /// Resumes the player's agent to allow it to move.
    /// Changes the player's layer back to Player to make it detectable by enemies.
    /// </summary>
    public void GetOut()
    {
        player.transform.position = enteredPosition;
        player.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;
        playerMovement.active = true;
        playerInteraction.enteredObject = null;
        player.layer = LayerMask.NameToLayer("Player");
        IsEmpty = true;
    }
}
