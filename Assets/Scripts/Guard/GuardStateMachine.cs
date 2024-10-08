using UnityEngine;

public class GuardStateMachine : MonoBehaviour
{
    private IGuardState currentState;

    void Start()
    {
        // Set the initial state.
        currentState = GetComponent<GuardPatrolState>();
        currentState.OnStateEnter();
    }

    // In each frame, update the current state.
    void Update()
    {
        if (currentState != null)
            currentState.UpdateState();
    }

    /// <summary>
    /// Sets a new state for the guard.
    /// It first exits the current state.
    /// </summary>
    /// <param name="newState">The new state to set.</param>
    public void SetState(IGuardState newState)
    {
        if (newState == null || newState == currentState)
            return;

        if (currentState != null)
            currentState.OnStateExit();

        currentState = newState;
        currentState.OnStateEnter();
    }
}

/// <summary>
/// An interface for a guard state.
/// It has methods for entering, updating, and exiting the state.
/// </summary>
public interface IGuardState
{
    void OnStateEnter();
    void UpdateState();
    void OnStateExit();
}
