using UnityEngine;

/// <summary>
/// This class is responsible for implementing what happens when the player is detected by an enemy.
/// </summary>
public class PlayerDetection : MonoBehaviour
{
    public bool isDetected = false;
    public Transform respawnPoint;

    /// <summary>
    /// Detects the player and stops the player's navigation.
    /// </summary>
    public void DetectPlayer()
    {
        isDetected = true;
        GetComponent<PlayerMovement>().CancelCurrentNavigation();
    }

    /// <summary>
    /// Undetects the player and resumes the player's navigation.
    /// </summary>
    public void UndetectPlayer()
    {
        isDetected = false;
        GetComponent<PlayerMovement>().ResumeAgent();
    }

    /// <summary>
    /// Respawns the player at the respawn point.
    /// Used after being caught by an enemy.
    /// </summary>
    public void RespawnPlayer()
    {
        transform.position = respawnPoint.position;
        transform.rotation = respawnPoint.rotation;
        GetComponent<PlayerMovement>().CancelCurrentNavigation();
    }
}
