using UnityEngine;

/// <summary>
/// ResolutionManager is a script that sets the resolution and refresh rate of the game.
/// It also provides functionality to set the game to fullscreen or windowed mode.
/// </summary>
/// <remarks> The ResolutionManager script is attached to any GameObject in the game to set the resolution of the game when it starts.</remarks>
public class ResolutionManager : MonoBehaviour
{
    public int width = 1920;
    public int height = 1080;
    public bool isFullScreen = true;
    public uint refreshRate = 60;

    void Start()
    {
        FullScreenMode mode = isFullScreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        Screen.SetResolution(width, height, mode, new RefreshRate() { numerator = refreshRate, denominator = 1 });
    }
}
