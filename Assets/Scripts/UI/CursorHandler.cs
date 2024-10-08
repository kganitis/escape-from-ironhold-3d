using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// CursorHandler is a script that handles the cursor changes in the game.
/// It is used to change the cursor based on the scene or the game state.
/// It provides different cursors for different interactions.
/// </summary>
public class CursorHandler : MonoBehaviour
{
    public Texture2D move;
    public Texture2D hand;
    public Texture2D arrow;
    public Texture2D defaultCursor;
    public Texture2D currentCursor;
    public CursorMode cursorMode = CursorMode.Auto;
    public Vector2 hotSpot = Vector2.zero;
    
    void Start()
    {
        // Check the current scene and set the cursor accordingly
        Scene currentScene = SceneManager.GetActiveScene();
        if (currentScene.name == "Ironhold")
        {
            SetMove();
        }
        else
        {
            SetHand();
        }
    }

    /// <summary>
    /// Sets the cursor to the move cursor.
    /// Also sets the hot spot to the center of the cursor.
    /// </summary>
    /// <remarks> The move cursor is used when the cursor hovers over an area where the player can move.</remarks>
    public void SetMove()
    {
        currentCursor = move;
        hotSpot = new Vector2(currentCursor.width / 2, currentCursor.height / 2);
        updateCursor();
    }

    /// <summary>
    /// Sets the cursor to the hand cursor.
    /// </summary>
    /// <remarks> The hand cursor is used when the cursor hovers over an interactable object.</remarks>
    public void SetHand()
    {
        currentCursor = hand;
        hotSpot = Vector2.zero;
        updateCursor();
    }

    /// <summary>
    /// Sets the cursor to the arrow cursor.
    /// </summary>
    /// <remarks> The arrow cursor is used when the cursor hovers over an interaction button.</remarks>
    public void SetArrow()
    {
        currentCursor = arrow;
        hotSpot = Vector2.zero;
        updateCursor();
    }

    /// <summary>
    /// Sets the cursor to the default cursor.
    /// </summary>
    public void SetDefault()
    {
        currentCursor = defaultCursor;
        if (defaultCursor == move)
        {
            hotSpot = new Vector2(currentCursor.width / 2, currentCursor.height / 2);
        }
        else
        {
            hotSpot = Vector2.zero;
        
        }
        updateCursor();
    }

    private void updateCursor()
    {
        Cursor.SetCursor(currentCursor, hotSpot, cursorMode);
    }
}
