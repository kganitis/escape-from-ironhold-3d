using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

/// <summary>
/// InteractionWindow is a script that displays interaction buttons for objects that can be interacted with.
/// It is used to display the available actions for the player to choose from.
/// </summary>
public class InteractionWindow : MonoBehaviour
{
    [SerializeField] GameObject interactionsPanel;
    private GameObject interactingObject;
    private List<GameObject> interactionButtons;
    public Attribute ChosenAction;

    void Start()
    {
        interactionButtons = new List<GameObject>
        {
            GameObject.Find("InteractionButton1"),
            GameObject.Find("InteractionButton2"),
            GameObject.Find("InteractionButton3")
        };

        Reset();
    }

    /// <summary>
    /// Displays a panel with the interaction buttons for the given object at the specified hit point.
    /// </summary>
    /// <param name="interactingObject">The object that the player is interacting with.</param>
    /// <param name="hitPoint">The point where the player clicked on the object.</param>
    public void DisplayInteractions(GameObject interactingObject, Vector3 hitPoint)
    {
        this.interactingObject = interactingObject;
        interactionsPanel.SetActive(true);
        SetButtonTexts();
        SetPosition(hitPoint);
    }

    /// <summary>
    /// Sets the text of the interaction buttons based on the atrributes of the interacting object.
    /// </summary>
    private void SetButtonTexts()
    {
        Attribute[] attributes = interactingObject.GetComponents<Attribute>();
        List<Attribute> validAttributes = new();

        foreach (Attribute attribute in attributes)
        {
            if (attribute.IsValid())
                validAttributes.Add(attribute);
        }

        for (int i = 0; i < interactionButtons.Count; i++) 
        {
            Attribute assingedAction = null;
            string interactionText = string.Empty;

            if (i < validAttributes.Count)
            {
                assingedAction = validAttributes[i];
                interactionText = validAttributes[i].ActionName;
            }

            interactionButtons[i].GetComponent<AssignAction>().AssignedAction = assingedAction;
            interactionButtons[i].GetComponentInChildren<Text>().text = interactionText;
        }
    }

    /// <summary>
    /// Sets the position of the interaction panel to the specified hit point.
    /// </summary>
    /// <param name="hitPoint">The point where the player clicked on the object.</param>
    /// <remarks> The hit point is converted from world space to screen space.</remarks>
    public void SetPosition(Vector3 hitPoint)
    {
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(hitPoint);
        RectTransform panelRect = interactionsPanel.GetComponent<RectTransform>();
        panelRect.anchoredPosition = screenPoint;
    }

    /// <summary>
    /// Gets the interaction button at the specified hit point.
    /// </summary>
    /// <param name="hitPoint">The point where the player clicked on the object.</param>
    /// <returns>The interaction button at the hit point, or null if no button is found.</returns>
    /// <remarks> The hit point is converted from world space to screen space
    /// and then to local space of the interaction button's RectTransform.
    /// The button is found if the local point is contained within the button's RectTransform.</remarks>
    public GameObject GetInteractionButtonAtPosition(Vector3 hitPoint)
    {
        if (!interactionsPanel.activeSelf)
        {
            return null;
        }

        // Convert hit point from world space to point in screen space
        Vector2 screenPoint = Camera.main.WorldToScreenPoint(hitPoint);

        foreach (GameObject button in interactionButtons)
        {
            // Convert screen point to local space of the interaction button's RectTransform
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                button.transform as RectTransform,
                screenPoint,
                null,
                out Vector2 localPoint
            );

            // Check if the button contains the local point
            RectTransform buttonRect = button.GetComponent<RectTransform>();
            if (buttonRect.rect.Contains(localPoint))
            {
                return button;
            }
        }

        return null;
    }

    /// <summary>
    /// Resets the interaction window.
    /// </summary>
    /// <remarks> The interaction panel is hidden and the interacting object is set to null.</remarks>
    public void Reset()
    {
        interactionsPanel.SetActive(false);
        
        if (interactingObject != null)
        {
            interactingObject.GetComponent<ObjectHighlighter>()?.ResetObjectHighlight(); 
            interactingObject = null;
        }
    }

    /// <summary>
    /// Checks if the interaction window is currently displayed.
    /// </summary>
    /// <returns>True if the interaction window is displayed, false otherwise.</returns>
    /// <remarks> The interaction window is considered displayed if the interaction panel is active.</remarks>
    public bool IsDisplayed()
    {
        return interactionsPanel.activeSelf;
    }

    /// <summary>
    /// Checks if the interaction window is currently displayed for the specified game object.
    /// </summary>
    /// <param name="gameObject">The game object to check if the interaction window is displayed for.</param>
    /// <returns>True if the interaction window is displayed for the game object, false otherwise.</returns>
    /// <remarks> The interaction window is considered displayed for the game object if the interaction panel is active and the game object is the interacting object.</remarks>
    public bool IsDisplayedForGameObject(GameObject gameObject)
    {
        return interactionsPanel.activeSelf && gameObject == interactingObject;
    }
}
