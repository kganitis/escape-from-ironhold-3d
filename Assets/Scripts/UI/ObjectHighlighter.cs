using UnityEngine;

/// <summary>
/// ObjectHighlighter is a script that highlights an object when the mouse hovers over it.
/// It is used to provide visual feedback to the player when an object is interactable.
/// It also changes the cursor to a hand cursor when the mouse hovers over the object.
/// </summary>
public class ObjectHighlighter : MonoBehaviour
{
    CursorHandler cursorHandler;
    PlayerController playerController;
    InteractionWindow interactionWindow;
    Renderer objectRenderer;
    Color startColor;
    public Color highlightColor = Color.red;

    void Start()
    {
        cursorHandler = GameObject.Find("Canvas").GetComponent<CursorHandler>();
        playerController = GameObject.Find("Player").GetComponent<PlayerController>();
        interactionWindow = GameObject.Find("Canvas").GetComponent<InteractionWindow>();
        objectRenderer = GetComponent<Renderer>();
        if (objectRenderer == null)
        {
            objectRenderer = GetComponentInChildren<SkinnedMeshRenderer>();
        }
        startColor = objectRenderer.material.color;
    }

    /// <summary>
    /// Highlights the object with the given color.
    /// </summary>
    /// <param name="highlightColor">The color to highlight the object with.</param>
    /// <remarks> The object is highlighted by changing its material color to the given color.</remarks>
    public void HighlightObject(Color highlightColor)
    {
        objectRenderer.material.color = highlightColor;
    }

    /// <summary>
    /// Resets the object highlight to its original color.
    /// </summary>
    /// <remarks> The object's material color is reset to the color it had before highlighting.</remarks>
    public void ResetObjectHighlight()
    {
        objectRenderer.material.color = startColor;
    }

    void OnMouseEnter()
    {
        cursorHandler.SetHand();
        HighlightObject(highlightColor);
    }

    void OnMouseExit()
    {
        cursorHandler.SetDefault();

        // Keep the object highlighted if the interaction window is displayed for it
        if (interactionWindow.IsDisplayedForGameObject(gameObject)) return;
        
        ResetObjectHighlight();
    }
}
