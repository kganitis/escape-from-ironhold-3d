using UnityEngine;

/// <summary>
/// This class is responsible to handle the player's input.
/// It checks if the player clicked on an interaction button, an interactable object or the ground.
/// It then performs the appropriate action.
/// </summary>
public class PlayerController : MonoBehaviour
{
    // References
    [SerializeField] private MessageHandler messageHandler;

    private PlayerAnimation anim;
    private PlayerInteraction interaction;
    private PlayerMovement movement;
    private PlayerDetection detection;

    // Fields
    [SerializeField] private LayerMask clickableLayers;
    private CustomActions inputActions;

    public bool active = true;

    private void Awake()
    {
        anim = GetComponent<PlayerAnimation>();
        interaction = GetComponent<PlayerInteraction>();
        movement = GetComponent<PlayerMovement>();
        detection = GetComponent<PlayerDetection>();

        inputActions = new CustomActions();
        inputActions.Main.Move.performed += _ => OnClick();
    }

    private void OnEnable() => inputActions.Enable();
    private void OnDisable() => inputActions.Disable();

    /// <summary>
    /// This method is called when the player clicks on the screen.
    /// Nothing will happen if the player is already interacting with an object
    /// or if the player is detected by an enemy.
    /// </summary>
    private void OnClick()
    {
        if (!active || anim.isAnimationPlaying || detection.isDetected)
            return;

        if (messageHandler.IsMessageDisplayed())
            messageHandler.HideMessage();

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100, clickableLayers))
        {
            OnRaycastHit(hit);
        }
    }

    /// <summary>
    /// This method is called when the player clicks on the screen.
    /// It checks if the player clicked on an interaction button, an interactable object or the ground.
    /// It then performs the appropriate action.
    /// </summary>
    /// <param name="hit">The raycast hit information.</param>
    private void OnRaycastHit(RaycastHit hit)
    {
        // Check if the player clicked on an interaction button
        GameObject interactionButton = interaction.interactionWindow.GetInteractionButtonAtPosition(hit.point);
        if (interaction.currentInteractable != null && interactionButton != null)
        {
            OnInteractionButtonClick(interactionButton);
            return;
        }

         // Check if the player clicked on an interactable object
        Interactable interactableObject = hit.collider.GetComponent<Interactable>();
        if (interactableObject != null)
        {
            // If entered an object, can only interact with that object
            if (interaction.enteredObject != null && interaction.enteredObject != interactableObject.gameObject)
                return;

            interaction.DisplayInteractionOptions(interactableObject, hit.point);
            return;
        }

         // If the player clicked on the ground
        interaction.interactionWindow.Reset();
        if (movement.active)
            movement.NavigateToPosition(hit.point);
    }

    /// <summary>
    /// This method is called when the player clicks on an interaction button.
    /// It will reset the button's highlight and the interaction window.
    /// It will then set the action to perform and navigate the player to the interaction position.
    /// </summary>
    /// <param name="interactionButton">The button the player clicked on.</param>
    private void OnInteractionButtonClick(GameObject interactionButton)
    {
        interactionButton.GetComponent<ButtonHighlighter>().ResetButtonHighlight();
        interaction.interactionWindow.Reset();

        interaction.actionToPerform = interactionButton.GetComponent<AssignAction>().AssignedAction;

        movement.NavigateToObjectPosition(interaction.GetInteractionPosition());
    }
}
