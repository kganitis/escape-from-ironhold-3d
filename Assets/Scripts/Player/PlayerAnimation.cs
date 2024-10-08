using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This class is responsible for the player's animations.
/// </summary>
public class PlayerAnimation : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    internal bool isAnimationPlaying = false;

    private const string IDLE = "Idle";
    private const string WALK = "Walk";

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        UpdateAnimations();
    }

    /// <summary>
    /// Determines which animation should be played based on the player's velocity.
    /// If the player is not moving, the idle animation will be played.
    /// If the player is moving, the walk animation will be played.
    /// If an animation is in progress, it will wait for it to finish and not play another one.
    /// </summary>
    private void UpdateAnimations()
    {
        // If an animation is in progress, wait for it to finish
        if (isAnimationPlaying)
        {
            // If the animation becomes idle, mark it as complete
            if (animator.GetNextAnimatorStateInfo(0).IsName(IDLE))
            {
                isAnimationPlaying = false;
            }
            return;
        }

        animator.Play(agent.velocity == Vector3.zero ? IDLE : WALK);
    }

    /// <summary>
    /// Plays the animation of the interaction.
    /// If the animation is "Idle", this method will do nothing.
    /// </summary>
    /// <param name="interaction">The interaction of which the animation should be played.</param>
    public void PlayInteractionAnimation(Attribute interaction)
    {
        if (interaction.Animation == IDLE) return;

        animator.Play(interaction.Animation);
        isAnimationPlaying = true;
    }

    /// <summary>
    /// Cancels the current animation and plays the idle animation.
    /// </summary>
    public void CancelCurrentAnimation()
    {
        animator.Play(IDLE);
        isAnimationPlaying = false;
    }
}

