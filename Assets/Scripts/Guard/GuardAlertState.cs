using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// A class that represents the alert state for a guard.
/// It's a sub-state of the patrol state, with different patrol points and behavior.
/// </summary>
public class GuardAlertState : GuardPatrolState
{
    #region Fields

    public GameObject alertSignPrefab;
    private GameObject alertSignInstance;

    #endregion
    
    #region IGuardState implementation

    /// <summary>
    /// Initializes the state.
    /// Ignores the open door when the guard is in alert mode.
    /// Shows an alert sign above the guard's head and displays a message.
    /// </summary>
    public override void OnStateEnter()
    {
        // Ignore the open door when the guard is in alert mode
        fieldOfView.IgnoreTarget(cellDoor.transform);
        cellDoor.layer = 0;

        alertSignInstance = ShowSignAboveHead(alertSignPrefab, new Vector3(0f, 180f, 0));
        messageHandler.DisplayMessage("Huh? The door is open!", 3f);

        currentPatrolIndex = 0;
        DetermineNextPatrolPoint();
        NavigateToNextPatrolPoint();
    }

    /// <summary>
    /// Exits the state.
    /// Destroys the alert sign above the guard's head.
    /// </summary>
    public override void OnStateExit()
    {
        Destroy(alertSignInstance);
    }

    #endregion

    #region State Logic

    /// <summary>
    /// Navigate to the next patrol point without pausing.
    /// </summary>
    protected override void OnReachPatrolPoint()
    {
        DetermineNextPatrolPoint();
        NavigateToNextPatrolPoint();
    }

    protected override void OnSpotOpenDoor() {}

    #endregion
}
