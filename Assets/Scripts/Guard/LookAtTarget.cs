using UnityEngine;

/// <summary>
/// A class that represents a patrol point for a guard.
/// It has a look target that the guard will look at when it reaches the point.
public class LookAtTarget : MonoBehaviour
{
    public Transform lookTarget;

    void OnDrawGizmos()
    {
        if (lookTarget != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawLine(transform.position, lookTarget.position);
        }
    }
}
