using UnityEngine;

/// <summary>
/// CameraController is a script that follows a target object with a smooth movement.
/// </summary>
public class CameraController : MonoBehaviour
{
    public Transform target;
    public float smoothSpeed = 8f;
    public Vector3 offset;

    // Implementation in LateUpdate because it tracks objects that might have moved inside Update.
    void LateUpdate()
    {
        if (target == null) return;

        Vector3 desiredPosition = new Vector3(target.position.x + offset.x, target.position.y + offset.y, target.position.z + offset.z);
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
