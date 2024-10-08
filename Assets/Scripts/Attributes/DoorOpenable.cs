using UnityEngine;

/// <summary>
/// This class is used to mark doors that can be opened or closed by the player.
/// It also adds the ability to rotate the door when opening or closing it.
/// </summary>
[RequireComponent(typeof(Unlockable))]
public class DoorOpenable : Openable
{
    private bool isOpening = false; // Flag to check if the door is opening
    private bool isClosing = false; // Flag to check if the door is closing
    public float rotatingSpeed = 90f; // Degrees per second
    public float rotationAmount = 135f; // Max rotation degrees
    private float currentRotationAmount = 0f; // Track the amount rotated

    void OnValidate()
    {
        if (rotatingSpeed < 0)
        {
            Debug.LogWarning("Rotating speed must not be negative. Setting to default value of 90.");
            rotatingSpeed = 90f;
        }
    }

    // On update, rotate the door object when it's opening or closing.
    void Update()
    {
        if (isOpening)
            RotateDoor(true);
        else if (isClosing)
            RotateDoor(false);
    }

    /// <summary>
    /// Rotates the door object when opening or closing it.
    /// The rotation is done in increments to simulate the door moving.
    /// When the door reaches the target rotation amount, the rotation is stopped.
    /// The door is also set to the correct rotation if there's any overshoot.
    /// </summary>
    /// <param name="opening">True if the door is opening, false if it's closing</param>
    private void RotateDoor(bool opening)
    {
        float rotationThisFrame = rotatingSpeed * Time.deltaTime;
        float targetRotationAmount = Mathf.Abs(rotationAmount);

        if (opening)
        {
            if (currentRotationAmount + rotationThisFrame > targetRotationAmount)
                rotationThisFrame = targetRotationAmount - currentRotationAmount;

            currentRotationAmount += rotationThisFrame;
            transform.Rotate(0f, Mathf.Sign(rotationAmount) * rotationThisFrame, 0f);

            if (Mathf.Abs(currentRotationAmount - targetRotationAmount) < 0.01f)
                FinishRotation(true);
        }
        else
        {
            if (currentRotationAmount - rotationThisFrame < 0f)
                rotationThisFrame = currentRotationAmount;

            currentRotationAmount -= rotationThisFrame;
            transform.Rotate(0f, -Mathf.Sign(rotationAmount) * rotationThisFrame, 0f);

            if (Mathf.Abs(currentRotationAmount) < 0.01f)
                FinishRotation(false);
        }
    }

    /// <summary>
    /// Finishes the rotation of the door object when opening or closing it,
    /// by setting the flags to false and correcting the rotation if there's any overshoot.
    /// </summary>
    /// <param name="opening">True if the door is opening, false if it's closing</param>
    private void FinishRotation(bool opening)
    {
        isOpening = false;
        isClosing = false;

        if (opening)
        {
            float overshoot = currentRotationAmount - Mathf.Abs(rotationAmount);
            transform.Rotate(0f, -Mathf.Sign(rotationAmount) * overshoot, 0f);
            currentRotationAmount = Mathf.Abs(rotationAmount);
        }
        else
        {
            float overshoot = currentRotationAmount;
            transform.Rotate(0f, Mathf.Sign(rotationAmount) * overshoot, 0f);
            currentRotationAmount = 0f;
            player.layer = LayerMask.NameToLayer("Player");
        }
    }

    /// <summary>
    /// Checks if the door is locked.
    /// </summary>
    /// <returns>True if the door is locked, false otherwise</returns>
    public bool IsLocked
    {
        get
        {
            bool isLocked = false;
            if (TryGetComponent(out Unlockable u))
                isLocked = u.IsLocked;
            return isLocked;
        }
        set
        {
            if (TryGetComponent(out Unlockable u))
                u.IsLocked = value;
        }
    }

    /// <summary>
    /// Checks if the action is valid.
    /// The action is valid if the door is not locked, not opening, and not closing.
    /// </summary>
    /// <returns>True if the action is valid, false otherwise</returns>
    public override bool IsValid()
    {
        return !IsLocked && !isOpening && !isClosing;
    }

    /// <summary>
    /// Opens the door and marks it as opening.
    /// It also sets the door layer to "Detectable".
    /// </summary>
    /// <remarks>Setting the layer to "Detectable" allows the guard to detect the door.</remarks>
    public override void Open()
    {
        if (IsOpen || isOpening) return;

        IsOpen = true;
        isOpening = true;
        isClosing = false;
        SetLayer("Detectable");
    }

    /// <summary>
    /// Closes the door and marks it as closing.
    /// It also sets the door layer to "Obstacle".
    /// </summary>
    /// <remarks>Setting the layer to "Obstacle" prevents the guard from detecting the door.</remarks>
    public override void Close()
    {
        if (!IsOpen || isClosing) return;

        player.layer = LayerMask.NameToLayer("Ignore Collisions");
        IsOpen = false;
        isClosing = true;
        isOpening = false;
        SetLayer("Obstacle");
    }

    /// <summary>
    /// Closes the door immediately and locks it.
    /// </summary>
    public void CloseImmediately(bool locked)
    {
        IsLocked = locked;

        if (!IsOpen) return;

        IsOpen = false;
        transform.Rotate(0f, -Mathf.Sign(rotationAmount) * currentRotationAmount, 0f);
        currentRotationAmount = 0f;
        SetLayer("Obstacle");
    }

    /// <summary>
    /// Used to set the layer of the door object.
    /// </summary>
    private void SetLayer(string layerName)
    {
        gameObject.layer = LayerMask.NameToLayer(layerName);
    }
}
