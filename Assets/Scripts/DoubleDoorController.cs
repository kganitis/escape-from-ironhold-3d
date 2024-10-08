using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A class that represents a double door controller.
/// It is used to control two doors that are linked together.
/// When one door is opened, the other door is also opened.
/// When one door is unlocked, the other door is also unlocked.
/// </summary>
public class DoubleDoorController : MonoBehaviour
{
    public Transform otherDoor;

    private Unlockable otherUnlockable;
    private DoorOpenable otherDoorOpenable;

    void Start()
    {
        if (otherDoor == null) return;

        otherUnlockable = otherDoor.GetComponent<Unlockable>();
        otherDoorOpenable = otherDoor.GetComponent<DoorOpenable>();

        if (otherUnlockable != null)
        {
            otherUnlockable.Executed += OnOtherDoorUnlock;
        }

        if (otherDoorOpenable != null)
        {
            otherDoorOpenable.Executed += OnOtherDoorOpen;
        }
    }

    /// <summary>
    /// Handles the event when the other door is opened.
    /// Opens or closes the door based on the state of the other door.
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void OnOtherDoorOpen(object sender, EventArgs e)
    {
        if (TryGetComponent(out DoorOpenable o))
        {
            if (otherDoorOpenable.IsOpen)
            {
                o.Open();
            }
            else
            {
                o.Close();
            }
        }
    }

    /// <summary>
    /// Handles the event when the other door is unlocked.
    /// Unlocks the door.
    /// </summary>
    private void OnOtherDoorUnlock(object sender, EventArgs e)
    {
        if (TryGetComponent(out Unlockable u))
        {
            u.IsLocked = false;
        }
    }
}
