using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Moves the object to a new position when the camera switches to a new angle.
/// </summary>
public class MoveOnCameraSwitch : MonoBehaviour
{
    // A struct that stores a rotation and a position.
    [Serializable]
    public struct RotationPositionPair
    {
        public Vector3 rotation;
        public Vector3 position;
    }

    public List<RotationPositionPair> rotationPositionPairs = new List<RotationPositionPair>(); 

    private Dictionary<Vector3, Vector3> rotationToPositionMap = new Dictionary<Vector3, Vector3>();

    void OnEnable()
    {
        InitializeDictionary();
        CameraAngleSwitcher.OnCameraSwitch += MoveObject;
    }

    void OnDisable()
    {
        CameraAngleSwitcher.OnCameraSwitch -= MoveObject;
    }

    private void InitializeDictionary()
    {
        foreach (var pair in rotationPositionPairs)
        {
            rotationToPositionMap[pair.rotation] = pair.position;
        }
    }

    /// <summary>
    /// Moves the object to a new position based on the new rotation.
    /// </summary>
    /// <param name="newRotation">The new rotation of the camera.</param>
    private void MoveObject(Vector3 newRotation)
    {
        if (rotationToPositionMap.ContainsKey(newRotation))
        {
            transform.position = rotationToPositionMap[newRotation];
        }
    }
}
