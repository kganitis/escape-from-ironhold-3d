using UnityEngine;
using System;

/// <summary>
/// CameraAngleSwitcher is a script that switches the camera angle between two positions.
/// It is triggered by a player entering a trigger collider.
/// </summary>
public class CameraAngleSwitcher : MonoBehaviour
{
    public Transform player;
    public GameObject cameraPivot;

    public Vector3 insideRotation = new Vector3(60, 180, 0);
    public Vector3 outsideRotation = new Vector3(60, 90, 0);

    public Vector3 insideOffset = new Vector3(0, 5, 0);
    public Vector3 outsideOffset = new Vector3(0, 0, 0);
    
    public Vector3 exitBoundary;
    public Vector3 enterBoundary;

    public Vector3 currentRotation;

    public static event Action<Vector3> OnCameraSwitch;

    void Start()
    {
        currentRotation = insideRotation;
        exitBoundary = transform.position;
        enterBoundary = new Vector3(transform.position.x, transform.position.y, -transform.position.z);
    }

    /*
     * It checks if the player has entered the trigger and switches the camera angle.
     */
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform == player)
        {
            Switch();
        }
    }

    /// <summary>
    /// It switches the camera angle between inside and outside the room.
    /// </summary>
    /// <remarks> It is called when the player enters the trigger collider.</remarks>
    public void Switch()
    {
        if (currentRotation == insideRotation)
        {
            SetOutsideRotation();
        }
        else if (currentRotation == outsideRotation)
        {
            SetInsideRotation();
        }
    }

    /// <summary>
    /// It sets the camera angle to the outside of the room.
    /// </summary>
    public void SetOutsideRotation()
    {
        SetRotation(outsideRotation);
        cameraPivot.GetComponent<CameraController>().offset = outsideOffset;
        transform.position = enterBoundary;
        OnCameraSwitch?.Invoke(outsideRotation);
    }

    /// <summary>
    /// It sets the camera angle to the inside of the room.
    /// </summary>
    public void SetInsideRotation()
    {
        SetRotation(insideRotation);
        cameraPivot.GetComponent<CameraController>().offset = insideOffset;
        transform.position = exitBoundary;
        OnCameraSwitch?.Invoke(insideRotation);
    }

    /// <summary>
    /// It sets the camera rotation.
    /// </summary>
    /// <param name="rotation">The rotation to set.</param>
    private void SetRotation(Vector3 rotation)
    {
        cameraPivot.transform.eulerAngles = rotation;
        currentRotation = rotation;
    }
}
