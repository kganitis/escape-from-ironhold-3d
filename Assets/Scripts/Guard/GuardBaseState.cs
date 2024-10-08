using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// An abstract class that represents a base state for a guard.
/// It contains common references to other components and utility methods.
/// </summary>
public abstract class GuardBaseState : MonoBehaviour, IGuardState
{
    protected GuardStateMachine stateMachine;
    protected NavMeshAgent agent;
    protected Animator animator;
    protected FieldOfView fieldOfView;
    protected GameObject player;
    protected PlayerDetection playerDetection;
    protected MessageHandler messageHandler;
    protected GameObject cellDoor;
    
    protected float lookRotationSpeed = 8f;

    protected virtual void Start()
    {
        InitializeComponents();
    }

    protected virtual void InitializeComponents()
    {
        stateMachine = GetComponent<GuardStateMachine>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        fieldOfView = GetComponent<FieldOfView>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerDetection = player.GetComponent<PlayerDetection>();
        messageHandler = GameObject.FindGameObjectWithTag("Canvas").GetComponent<MessageHandler>();
        cellDoor = GameObject.FindGameObjectWithTag("CellDoor");
    }

    public abstract void OnStateEnter();

    public abstract void UpdateState();

    public abstract void OnStateExit();

     /// <summary>
     /// Makes the game object turn to face the target smoothly by the factor of lookRotationSpeed.
     /// </summary>
     /// <param name="targetPosition">The position of the target to face.</param>
    protected void FaceTarget(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * lookRotationSpeed);
    }

     /// <summary>
     /// Updates the animation based on the agent's velocity.
     /// </summary>
    protected void UpdateAnimation()
    {
        if (agent.velocity == Vector3.zero)
            animator.Play("Idle");
        else
            animator.Play("Patrol");
    }

     /// <summary>
     /// Shows a sign above the guard's head.
     /// The sign is a 3D model that is instantiated and parented to the guard's head.
     /// </summary>
     /// <param name="signPrefab"> The sign prefab to show. </param>
     /// <param name="rotationOffset"> An optional rotation offset, in case the sign needs to be rotated. </param>
     /// <returns> The sign instance. </returns>
    protected GameObject ShowSignAboveHead(GameObject signPrefab, Vector3 rotationOffset = default)
    {
        if (signPrefab != null)
        {
            Vector3 position = transform.position + Vector3.up * 4;
            GameObject signInstance = Instantiate(signPrefab, position, Quaternion.identity);
            signInstance.transform.localScale = new Vector3(3, 3, 3);
            signInstance.transform.SetParent(transform);
            var faceCamera = signInstance.AddComponent<FaceCamera>();
            faceCamera.rotationOffset = rotationOffset;
            return signInstance;
        }
        return null;
    }
}
