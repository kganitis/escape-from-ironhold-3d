using UnityEngine;
using System.Collections;

/// <summary>
/// A class that represents the sleep state for a guard.
/// It makes the guard sleep for a certain duration.
/// </summary>
public class GuardSleepState : GuardBaseState, IGuardState
{
    #region Fields

    public float chanceToSleep = 0.25f;
    public float sleepDuration = 5f;

    #endregion
    
    #region IGuardState implementation

    public override void OnStateEnter()
    {
        if (sleepDuration <= 0f)
        {
            stateMachine.SetState(GetComponent<GuardPatrolState>());
            return;
        }

        fieldOfView.Deactivate();
        StartCoroutine(SleepRoutine());
    }

    public override void UpdateState() {}

    public override void OnStateExit()
    {
        fieldOfView.Activate();
    }

    #endregion

    #region State Logic

     /// <summary>
     /// A coroutine that implements the sleep logic.
     /// Plays the sleep animation, waits for the sleep duration factor, and then transitions to the Patrol state.
     /// The guard's collider is reshaped to match the guard's new body positioning, and then reshaped back to normal.
     /// </summary>
    private IEnumerator SleepRoutine()
    {
        animator.SetTrigger("FallAsleepTrigger");
    
        // Wait for the sleep animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 3.5f);
    
        ReshapeCollider(1, 0.5f, 2);
    
        yield return new WaitForSeconds(sleepDuration);
    
        animator.SetTrigger("WakeUpTrigger");
    
        // Wait for the wake up animation to finish
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length * 2);
    
        ReshapeCollider(0.5f, 3, 1);
    
        // Transition to the next state after a short delay
        yield return new WaitForSeconds(2);
        stateMachine.SetState(GetComponent<GuardPatrolState>());
    }

    /// <summary>
    /// Reshape the guard's collider.
    /// </summary>
    /// <param name="radius"> The radius of the collider. </param>
    /// <param name="height"> The height of the collider. </param>
    /// <param name="direction"> The direction of the collider. </param>
    /// <remarks>Used to reshape the guard's collider when the guard is sleeping.</remarks>
    private void ReshapeCollider(float radius, float height, int direction)
    {
        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        capsuleCollider.radius = radius;
        capsuleCollider.height = height;
        capsuleCollider.direction = direction;
    }

    #endregion
}
