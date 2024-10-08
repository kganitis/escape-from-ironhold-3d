using UnityEngine;

/// <summary>
/// FaceCamera is a script that makes an object face the camera.
/// It is useful for UI elements that should always face the camera.
/// </summary>
public class FaceCamera : MonoBehaviour
{
    private Transform mainCamera;
    public Vector3 rotationOffset = new Vector3(0, 0, 0);

    void Start()
    {
        mainCamera = Camera.main.transform;
    }

    void LateUpdate()
    {
        transform.LookAt(transform.position + mainCamera.rotation * Vector3.forward, mainCamera.rotation * Vector3.up);
        transform.Rotate(rotationOffset);
    }
}
