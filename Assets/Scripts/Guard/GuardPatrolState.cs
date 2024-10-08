using UnityEngine;
using UnityEngine.AI;
using System.Collections;

/// <summary>
/// A class that represents the patrol state for a guard.
/// It makes the guard move between a set of patrol points.
/// It changes the state to GuardPauseState when the guard reaches a patrol point.
/// It changes the state to GuardAlertState when the guard spots the door open.
/// It changes the state to GuardPlayerDetectedState when the guard spots the player.
/// </summary>
public class GuardPatrolState : GuardBaseState, IGuardState
{
    #region Fields

    public Transform[] patrolPoints;

    internal int currentPatrolIndex = 0;
    internal Transform currentPatrolPoint => patrolPoints[currentPatrolIndex];

    private bool movingForward = true; // Whether the guard is moving forward or backwards in the patrol points array

    #endregion

    #region IGuardState implementation

    /// <summary>
    /// Initializes the state.
    /// Sets the destination to the first patrol point.
    /// </summary>
    public override void OnStateEnter()
    {
        DetermineNextPatrolPoint();
        NavigateToNextPatrolPoint();
    }

    /// <summary>
    /// Updates the state.
    /// Faces the target, updates the animation, and checks for the following:
    /// - If the guard has reached the patrol point
    /// - If the guard spots the door open
    /// - If the guard spots the player
    /// If any of the conditions are met, the state is changed accordingly.
    /// </summary>
    public override void UpdateState()
    {
        FaceTarget(agent.destination);
        UpdateAnimation();

        // If the guard has reached the patrol point
        if (!agent.pathPending && agent.remainingDistance < 0.1f)
        {
            agent.destination = transform.position; // Stop the agent
            OnReachPatrolPoint();
            return;
        }

        // If the guard spots the door open
        if (fieldOfView.IsTargetVisible(cellDoor.transform))
        {
            OnSpotOpenDoor();
            return;
        }

        // If the guard spots the player
        if (fieldOfView.IsTargetVisible(player.transform))
        {
            stateMachine.SetState(GetComponent<GuardPlayerDetectedState>());
            return;
        }
    }

    public override void OnStateExit() {}

    #endregion

    #region State Logic

     /// <summary>
     /// Navigates to the next patrol point.
     /// </summary>
    protected void NavigateToNextPatrolPoint()
    {
        agent.SetDestination(patrolPoints[currentPatrolIndex].position);
    }

     /// <summary>
     /// Determines the next patrol point based on the current index.
     /// If the guard has reached the end of the patrol points array,
     /// the order is reversed.
     /// </summary>
    protected void DetermineNextPatrolPoint()
    {
        if (movingForward)
        {
            currentPatrolIndex++;
            if (currentPatrolIndex >= patrolPoints.Length)
            {
                // If reached the end, start going backwards
                currentPatrolIndex = patrolPoints.Length - 2;
                movingForward = false;
            }
        }
        else
        {
            currentPatrolIndex--;
            if (currentPatrolIndex < 0)
            {
                // If reached the beginning, start going forwards
                currentPatrolIndex = 1;
                movingForward = true;
            }
        }
    }

    /// <summary>
    /// Changes the state to GuardPauseState when the guard reaches a patrol point.
    /// </summary>
    protected virtual void OnReachPatrolPoint()
    {
        stateMachine.SetState(GetComponent<GuardPauseState>());
    }

    /// <summary>
    /// Changes the state to GuardAlertState when the guard spots the door open.
    /// </summary>
    protected virtual void OnSpotOpenDoor()
    {
        stateMachine.SetState(GetComponent<GuardAlertState>());
    }

    #endregion

    protected void OnDrawGizmos()
    {
        if (patrolPoints == null || patrolPoints.Length == 0)
            return;

        Gizmos.color = Color.red;

        for (int i = 0; i < patrolPoints.Length; i++)
        {
            Gizmos.DrawSphere(patrolPoints[i].position, 0.2f);
            if (i < patrolPoints.Length - 1)
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[i + 1].position);
            }
            else
            {
                Gizmos.DrawLine(patrolPoints[i].position, patrolPoints[0].position);
            }
        }
    }
}
