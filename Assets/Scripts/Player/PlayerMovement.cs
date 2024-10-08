using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the player movement.
/// It uses the NavMeshAgent to navigate the player to a specific position.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    internal NavMeshAgent agent;

    private float lookRotationSpeed = 8f;
    [SerializeField] private ParticleSystem clickEffect;

    public bool active = true;
    
    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        FaceDestination();
        ResetPhysics();
    }

    /// <summary>
    /// This method is responsible for navigating the player to a specific position.
    /// It also instantiates a click effect in the destination.
    /// If the movement is not active, it will not navigate.
    /// </summary>
    /// <param name="position">The position the player should navigate to.</param>
    public void NavigateToPosition(Vector3 position)
    {
        if (!active)
            return;
    
        ResumeAgent();
        if (clickEffect != null)
        {
            Instantiate(clickEffect, position + new Vector3(0, 0.1f, 0), clickEffect.transform.rotation);
        }
        agent.destination = position;
    }

    /// <summary>
    /// This method is responsible for navigating the player to a specific object's position.
    /// If the movement is not active, it will not navigate.
    /// </summary>
    /// <param name="position">The position the player should navigate to.</param>
    public void NavigateToObjectPosition(Vector3 position)
    {
        if (!active)
            return;

        ResumeAgent();
        agent.destination = position;
    }

    /// <summary>
    /// Used to stop the agent's movement.
    /// </summary>
    public void PauseAgent()
    {
        CancelCurrentNavigation();
        agent.isStopped = true;
        agent.updateRotation = false;
    }

    /// <summary>
    /// Used to resume the agent's movement.
    /// </summary>
    public void ResumeAgent()
    {
        agent.isStopped = false;
        agent.updateRotation = true;
    }

    /// <summary>
    /// Cancels the current navigation and stops the agent's movement.
    /// </summary>
    public void CancelCurrentNavigation()
    {
        agent.destination = transform.position;
        GetComponent<PlayerAnimation>().CancelCurrentAnimation();
    }

    /// <summary>
    /// Makes the player face the destination of the navigation.
    /// Called in each frame in the Update method.
    /// </summary>
    private void FaceDestination()
    {
        if (agent.velocity == Vector3.zero) return;

        Vector3 direction = (agent.destination - transform.position).normalized;
        Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * lookRotationSpeed);
    }

    /// <summary>
    /// Resets the player's physics to prevent sliding.
    /// </summary>
    private void ResetPhysics()
    {
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb == null) return;

        rb.angularVelocity = Vector3.zero;
    }
}
