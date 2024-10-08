using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// ButtonHighlight is a script that changes the color of a button when the mouse hovers over it.
/// </summary>
/// <remarks> It is used to provide visual feedback to the player when they hover over a button.</remarks>
public class ButtonHighlighter : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private CursorHandler cursorHandler;
    private Text interactionText;
    private Color normalColor;
    private static readonly Color HighlightColor = new Color(1f, 0.92f, 0.016f);

    void Awake()
    {
        interactionText = GetComponentInChildren<Text>();
        normalColor = interactionText.color;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        HighlightButton();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ResetButtonHighlight();
    }

    /// <summary>
    /// Highlights the button by changing the color of the text.
    /// Ιt also changes the cursor to an arrow.
    /// </summary>
    public void HighlightButton()
    {
        if (cursorHandler != null)
        {
            cursorHandler.SetArrow();
        }

        interactionText.color = HighlightColor;
    }

    /// <summary>
    /// Resets the button highlight by changing the color of the text back to normal.
    /// Ιt also changes the cursor to the default.
    /// </summary>
    public void ResetButtonHighlight()
    {
        if (cursorHandler != null)
        {
            cursorHandler.SetDefault();
        }

        interactionText.color = normalColor;
    }
}
