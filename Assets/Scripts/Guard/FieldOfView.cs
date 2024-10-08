using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// A class that represents a field of view for an object.
/// It detects targets within a certain radius and angle.
/// It uses a layer mask to detect potential targets and obstacles.
/// </summary>
public class FieldOfView : MonoBehaviour
{
    public float viewRadius = 8f;
    [Range(0, 360)] public float viewAngle = 180f;
    public float detectionDelay = 1.0f;
    public LayerMask targetMask;
    public LayerMask obstacleMask;

    private List<Transform> visibleTargets = new List<Transform>();
    private Dictionary<Transform, float> targetDetectionTimes = new Dictionary<Transform, float>();
    

    private bool active = true;

    void Start()
    {
        StartCoroutine(FindTargetsWithDelay());
    }

    public void Activate()
    {
        Reset();
        active = true;
    }

    public void Deactivate()
    {
        Reset();
        active = false;
    }

    /// <summary>
    /// Clear the list of visible targets.
    /// </summary>
    public void Reset()
    {
        visibleTargets.Clear();
        targetDetectionTimes.Clear();
    }

    /// <summary>
    /// Remove a target from the list of visible targets.
    /// </summary>
    /// <param name="target">The target to be removed.</param>
    public void IgnoreTarget(Transform target)
    {
        visibleTargets.Remove(target);
        targetDetectionTimes.Remove(target);
    }

    /// <summary>
    /// Check if a target is in the list of visible targets.
    /// </summary>
    /// <param name="target">The target to check.</param>
    public bool IsTargetVisible(Transform target)
    {
        return visibleTargets.Contains(target);
    }

    /// <summary>
    /// Check if there are any visible targets.
    /// </summary>
    /// <returns>True if there are visible targets, false otherwise.</returns>
    public bool HasVisibleTargets()
    {
        return visibleTargets.Count > 0;
    }

    /// <summary>
    /// Get the list of visible targets.
    /// </summary>
    /// <returns>The list of visible targets.</returns>
    public List<Transform> GetVisibleTargets()
    {
        return new List<Transform>(visibleTargets);
    }

    /// <summary>
    /// Runs every 0.2 seconds to update the list of visible targets.
    /// </summary>
    IEnumerator FindTargetsWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.2f);
            UpdateVisibleTargets();
        }
    }

    /// <summary>
    /// Updates the list of visible targets based on detection time.
    /// If a target is in the field of view and not obstructed by obstacles,
    /// it is added to the list of visible targets, if it has been visible for long enough.
    /// If a target is no longer in the field of view, it is removed from the list of visible targets.
    /// </summary>
    void UpdateVisibleTargets()
    {
        if (!active)
            return;

        // Find all targets in the view radius
        Collider[] targetsInViewRadius = Physics.OverlapSphere(transform.position, viewRadius, targetMask);
        List<Transform> targetsOutOfView = new();

        // Check if the targets are in the field of view and not obstructed by obstacles
        for (int i = 0; i < targetsInViewRadius.Length; i++)
        {
            Transform target = targetsInViewRadius[i].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            // Check if the target is in the field of view
            bool targetInFieldOfView = Vector3.Angle(transform.forward, directionToTarget) < viewAngle / 2;
            if (!targetInFieldOfView)
            {
                targetsOutOfView.Add(target);
                continue;
            }
            
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            // Check if the target is obstructed by an obstacle
            bool targetObstructed = Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstacleMask);
            if (targetObstructed)
            {
                targetsOutOfView.Add(target);
                continue;
            }
            
            // Check if the target has been detected for the first time and begin counting the detection time
            bool detectedForFirstTime = !targetDetectionTimes.ContainsKey(target);
            if (detectedForFirstTime)
                targetDetectionTimes[target] = Time.time;

            // Check if the target has been visible for long enough and add it to the list of visible targets
            bool detectionTimeExpired = Time.time - targetDetectionTimes[target] >= detectionDelay;
            if (detectionTimeExpired && !visibleTargets.Contains(target))
                visibleTargets.Add(target);
        }

        // Remove targets that are no longer in view
        List<Transform> targetsToRemove = new List<Transform>();
        foreach (var kvp in targetDetectionTimes)
        {
            Transform target = kvp.Key;
            if (targetsOutOfView.Contains(target))
                targetsToRemove.Add(target);
        }

        foreach (Transform target in targetsToRemove)
        {
            targetDetectionTimes.Remove(target);
        }
    }

    // Used by FieldOfViewVisualizer to draw the field of view.
    internal Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
            angleInDegrees += transform.eulerAngles.y;

        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
