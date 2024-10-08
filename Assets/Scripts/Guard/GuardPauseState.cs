using UnityEngine;
using System.Collections;

public class GuardPauseState : GuardBaseState, IGuardState
{
    #region Fields

    public float pauseDuration = 5f;
    private Transform lookTarget;
    private float elapsedTime = 0f;

    #endregion
    
    #region IGuardState implementation

    public override void OnStateEnter()
    {
        if (pauseDuration <= 0f)
        {
            stateMachine.SetState(GetComponent<GuardPatrolState>());
            return;
        }
        
        var currentPatrolPoint = GetComponent<GuardPatrolState>().currentPatrolPoint;
        lookTarget = currentPatrolPoint.GetComponent<LookAtTarget>().lookTarget;
        elapsedTime = 0f;
    }

    /// <summary>
    /// Updates the state.
    /// Faces the target, plays the idle animation, and waits for the pause duration.
    /// The state transitions to the next state based on the following conditions:
    /// - If the guard spots the door open
    /// - If the guard spots the player
    /// If any of the conditions are met, the state is changed accordingly.
    /// If none of the conditions are met, the guard transitions to the next state once the pause duration is over.
    /// </summary>
    public override void UpdateState()
    {
        elapsedTime += Time.deltaTime;

        if (elapsedTime > pauseDuration)
        {
            TransitionToNextState();
            return;
        }

        FaceTarget(lookTarget.position);
        animator.Play("Idle");

        // If the guard spots the door open
        if (fieldOfView.IsTargetVisible(cellDoor.transform))
        {
            stateMachine.SetState(GetComponent<GuardAlertState>());
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
     /// Transitions randomly to the next state based on the guard's chance to sleep.
     /// The guard can either go to sleep or resume patrolling.
     /// </summary>
    private void TransitionToNextState()
    {
        if (UnityEngine.Random.value < GetComponent<GuardSleepState>().chanceToSleep)
            stateMachine.SetState(GetComponent<GuardSleepState>());
        else
            stateMachine.SetState(GetComponent<GuardPatrolState>());
    }

    #endregion
}
