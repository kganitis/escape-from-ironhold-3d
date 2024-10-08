using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// SceneLoader is a script that loads different scenes in the game.
/// It also provides functionality to quit the game.
/// </summary>
/// <remarks> The SceneLoader script is attached to UI buttons in the game to trigger scene changes.</remarks>
public class SceneLoader : MonoBehaviour
{
    /// <summary>
    /// Loads the main menu scene.
    /// </summary>
    /// <remarks> Used when the player clicks the "Main Menu" button.</remarks>
    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenuScene");
    }

    /// <summary>
    /// Loads the game scene.
    /// </summary>
    /// <remarks> Used when the player clicks the "Start Game" button.</remarks>
    public void StartGame()
    {
        SceneManager.LoadScene("Ironhold");
    }

    /// <summary>
    /// Quits the game.
    /// </summary>
    /// <remarks> Used when the player clicks the "Quit Game" button.</remarks>
    public void QuitGame()
    {
        Application.Quit();
    }

    /// <summary>
    /// Loads the about scene.
    /// </summary>
    /// <remarks> Used when the player clicks the "About" button.</remarks>
    public void About()
    {
        SceneManager.LoadScene("AboutScene");
    }

    /// <summary>
    /// Reloads the current scene.
    /// </summary>
    /// <remarks> Used when the player clicks the "Restart" button.</remarks>
    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}
