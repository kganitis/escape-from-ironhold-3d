using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents the player detected state for a guard.
/// It is used when the guard detects the player.
/// Handles the player detection and transitions to the patrol state.
/// </summary>
public class GuardPlayerDetectedState : GuardBaseState, IGuardState
{
    #region Fields & References

    // References
    [SerializeField] private CameraAngleSwitcher cameraAngleSwitcher;

    // Fields
    public Transform respawnPoint;

    public GameObject detectionSignPrefab;
    private GameObject detectionSignInstance;

    private float reachDelay = 2f; // Delay before the guard starts moving towards the player
    private float elapsedTime = 0f;
    private bool moveTowardsPlayer = false; // Used to ensure that the guard moves towards the player only once
    private bool reachedPlayer = false; // Used to flag when the guard has reached the player

    #endregion

    #region IGuardState implementation

    /// <summary>
    /// Initializes the state.
    /// Resets the state variables, marks the player as detected,
    /// deactivates the field of view, and displays a message.
    /// </summary>
    public override void OnStateEnter()
    {
        // Reset the state variables
        elapsedTime = 0f;
        moveTowardsPlayer = false;
        reachedPlayer = false;

        // Mark the player as detected
        playerDetection.DetectPlayer();

        // Stop the guard
        agent.destination = transform.position;

        // Deactivate the field of view since the player has been detected
        fieldOfView.Deactivate();

        // Inform the player that they have been detected
        detectionSignInstance = ShowSignAboveHead(detectionSignPrefab);
        messageHandler.DisplayMessage("Stop right there!");
    }

    /// <summary>
    /// Updates the state.
    /// Moves the guard towards the player after a delay.
    /// </summary>
    public override void UpdateState()
    {
        FaceTarget(player.transform.position);
        UpdateAnimation();

        // Move the guard towards the player after a delay
        elapsedTime += Time.deltaTime;
        if (elapsedTime < reachDelay)
            return;

        // Ensure that the destination is set only once
        if (!moveTowardsPlayer)
        {
            moveTowardsPlayer = true;
            agent.destination = player.transform.position;
        }

        // When the guard reaches the player's position
        if (!agent.pathPending && agent.remainingDistance < 1.25f && !reachedPlayer)
        {
            reachedPlayer = true;
            agent.destination = transform.position;
            StartCoroutine(OnReachPlayerAfterDelay());
            return;
        }
    }

    public override void OnStateExit()
    {
        agent.destination = transform.position;
        StartCoroutine(ReactivateFieldOfViewWithDelay());
    }

    #endregion

    #region State Logic

    /// <summary>
    /// A coroutine that waits for a delay before handling the player detection.
    /// Called when the guard reaches the player's position.
    /// </summary>
    IEnumerator OnReachPlayerAfterDelay()
    {
        yield return new WaitForSeconds(2);
        OnReachPlayer();
    }

    /// <summary>
    /// Handles the player detection.
    /// The player is respawned, the cell door is closed,
    /// and the camera angle is switched. The guard then transitions to the patrol state.
    /// </summary>
    private void OnReachPlayer()
    {
        messageHandler.HideMessage();
        Destroy(detectionSignInstance);

        this.Respawn();

        playerDetection.RespawnPlayer(); // Send the player back to the cell
        playerDetection.UndetectPlayer();
        cellDoor.GetComponent<DoorOpenable>().CloseImmediately(true);
        cameraAngleSwitcher.SetInsideRotation(); // Set the camera angle to the cell

        // Transition to the patrol state
        GetComponent<GuardPatrolState>().currentPatrolIndex = 0;
        stateMachine.SetState(GetComponent<GuardPatrolState>());
    }

    /// <summary>
    /// Respawns the guard at the respawn point.
    /// Used to reset the guard's position after the player has been caught.
    /// </summary>
    private void Respawn()
    {
        gameObject.transform.position = respawnPoint.position;
        gameObject.transform.rotation = respawnPoint.rotation;
    }

    /// <summary>
    /// Reactivates the field of view after a delay.
    /// </summary>
    private IEnumerator ReactivateFieldOfViewWithDelay()
    {
        yield return new WaitForSeconds(1);
        fieldOfView.Activate();
    }

    #endregion
}
