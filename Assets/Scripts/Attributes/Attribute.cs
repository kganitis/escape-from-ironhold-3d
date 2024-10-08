using UnityEngine;
using System;

/// <summary>
/// Base class for all attributes.
/// Attributes implement the logic for the player to interact with objects in the game.
/// </summary>
public abstract class Attribute : MonoBehaviour
{
    [Header("References")]
    private MessageHandler messageHandler;
    protected InventoryManager inventoryManager;
    protected GameObject player;
    protected PlayerMovement playerMovement;
    protected PlayerInteraction playerInteraction;

    [Header("Properties")]
    protected string actionName;
    protected string defaultAnimation = "Idle"; // Default animation to play when executing the action
    [SerializeField] private string _animation; // Specific animation to play when executing the action
    public Transform interactionPoint; // Point where the player should stand to interact with the object
    [HideInInspector] public bool Silent = false; // If true, the action will not display any message
    protected string itemTextColor = "#8b0000"; // Color of the item name in the message

    public event EventHandler Executed; // Event triggered when the action is executed
    
    public virtual string ActionName
    {
        get { return actionName; }
        set { actionName = value; }
    }

    public string Animation
    {
        get
        {
            if (string.IsNullOrEmpty(_animation))
            {
                return defaultAnimation;
            }
            else
            {
                return _animation;
            }
        }
    }

    protected virtual void Awake()
    {
        messageHandler = FindObjectOfType<MessageHandler>();
        inventoryManager = FindObjectOfType<InventoryManager>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerMovement = player.GetComponent<PlayerMovement>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
    }

    /// <summary>
    /// Displays a message on the screen.
    /// </summary>
    /// <param name="message">Message to display</param>
    /// <remarks>If the Silent property is true, the message will not be displayed.</remarks>
    protected void DisplayMessage(string message)
    {
        if (Silent) return;
        messageHandler.DisplayMessage(message);
    }

    /// <summary>
    /// Used to check if the action can be executed.
    /// </summary>
    /// <returns>True if the action can be executed, false otherwise</returns>
    public virtual bool IsValid()
    {
        return true;
    }

    /// <summary>
    /// Executes the action.
    /// </summary>
    /// <returns>True if the action was executed successfully, false otherwise</returns>
    /// <remarks>If the action was executed successfully, the Executed event will be triggered.</remarks>
    public virtual bool Execute()
    {
        Executed?.Invoke(this, EventArgs.Empty);
        return true;
    }
}