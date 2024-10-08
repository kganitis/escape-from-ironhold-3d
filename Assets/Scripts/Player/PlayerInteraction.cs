using UnityEngine;

/// <summary>
/// This class is responsible for the player's interactions.
/// It handles the interactions between the player and the interactable objects.
/// </summary>
public class PlayerInteraction : MonoBehaviour
{
    // References
    public InteractionWindow interactionWindow;
    [SerializeField] private CursorHandler cursorHandler;

    // Fields
    internal GameObject currentInteractable;
    internal Attribute actionToPerform;
    [HideInInspector] public GameObject enteredObject = null; // Used if the player has entered in another object through the 'Get in' action.

    private void OnTriggerEnter(Collider other) => OnTrigger(other);
    private void OnTriggerStay(Collider other) => OnTrigger(other);

    /// <summary>
    /// This method is called when the player is near an interactable object.
    /// If the object is the same as the current interactable object
    /// and an action has been set, it will execute the interaction.
    /// </summary>
    /// <param name="other">The collider of the object the player entered.</param>
    private void OnTrigger(Collider other)
    {
        if (other.gameObject == currentInteractable && actionToPerform != null)
        {
            ExecuteInteraction();
        }
    }

    /// <summary>
    /// This method is called when the player is near the current interactable object.
    /// It will stop the player's movement and execute the interaction.
    /// If the action is performed successfully, it will play the interaction animation.
    /// </summary>
    private void ExecuteInteraction()
    {
        GetComponent<PlayerMovement>().PauseAgent();
        FaceCurrentInteractable();

        if (actionToPerform.Execute())
        {
            GetComponent<PlayerAnimation>().PlayInteractionAnimation(actionToPerform);
        }

        actionToPerform = null;
        currentInteractable = null;
        cursorHandler.SetMove();
    }

    /// <summary>
    /// Makes the player turn immediately to face the current interactable object.
    /// </summary>
    private void FaceCurrentInteractable()
    {
        if (currentInteractable == null) return;

        Vector3 direction = (GetInteractionPosition() - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = targetRotation;
    }

    /// <summary>
    /// Finds the position at which the interaction should take place.
    /// </summary>
    /// <returns>A vector3 representing the position of the interaction.</returns>
    public Vector3 GetInteractionPosition()
    {
        Interactable obj = currentInteractable.GetComponent<Interactable>();
        if (obj != null && obj.interactionPoint != null)
        {
            return obj.interactionPoint.position;
        }
        else
        {
            return currentInteractable.transform.position;
        }
    }

    /// <summary>
    /// This method is called when the player clicks on an interactable object.
    /// It will display the interaction window with the available actions.
    /// </summary>
    /// <param name="interactableObject">The object the player clicked on.</param>
    /// <param name="hitPoint">The point where the player clicked on the object.</param>
    /// <remarks> The interaction window will be displayed at the hit point in screen space.</remarks>
    public void DisplayInteractionOptions(Interactable interactableObject, Vector3 hitPoint)
    {
        GetComponent<PlayerMovement>().PauseAgent();
        interactionWindow.Reset();
        currentInteractable = interactableObject.gameObject;
        interactionWindow.DisplayInteractions(currentInteractable, hitPoint);
    }
}
