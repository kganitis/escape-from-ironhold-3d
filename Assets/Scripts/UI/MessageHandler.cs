using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Handles the display of messages on the screen.
/// </summary>
/// <remarks> The message box is a child of the object this script is attached to.</remarks>
public class MessageHandler : MonoBehaviour
{
    [SerializeField] private GameObject messageBox;
    private Text messageText;

    void Start()
    {
        messageText = messageBox.GetComponentInChildren<Text>();
        HideMessage();
    }

    /// <summary>
    /// Hides the message box.
    /// </summary>
    public void HideMessage()
    {
        messageText.text = string.Empty;
        messageBox.SetActive(false);
    }

    /// <summary>
    /// Checks if the message box is currently displayed.
    /// </summary>
    /// <returns>True if the message box is displayed, false otherwise.</returns>
    public bool IsMessageDisplayed() => messageBox.activeSelf;

    /// <summary>
    /// Displays a message on the screen.
    /// </summary>
    /// <param name="message">The message to display.</param>
    /// <param name="displayDuration">The duration to display the message for. If 0, the message will be displayed indefinitely.</param>
    /// <remarks> If a message is already displayed, it will be replaced by the new message.
    /// If a display duration is provided, the message will be hidden after the duration has passed.</remarks>
    public void DisplayMessage(string message, float displayDuration = 0.0f)
    {
        messageText.text = message;
        messageBox.SetActive(true);

        // Start a coroutine to hide the message box after a delay
        if (displayDuration > 0)
        {
            StartCoroutine(HideMessageAfterDelay(displayDuration));
        }
    }

    /// <summary>
    /// Hides the message box after a delay.
    /// </summary>
    /// <param name="delay">The delay before hiding the message box.</param>
    /// <remarks> The message box will only be hidden if the message hasn't changed during the delay.</remarks>
    IEnumerator HideMessageAfterDelay(float delay)
    {
        string message = messageText.text;
        
        yield return new WaitForSeconds(delay);

        // Check if the message hasn't changed before hiding it
        if (messageText.text == message)
        {
            HideMessage();
        }
    }
    
}
