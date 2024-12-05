using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    // Name of the pause menu scene
    [SerializeField] private string pauseMenuSceneName = "Pause Menu";

    void Update()
    {
        // Check if the Escape key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadPauseMenu();
        }
    }

    // Load the pause menu scene
    private void LoadPauseMenu()
    {
        // Check if the scene is already loaded
        if (SceneManager.GetActiveScene().name != pauseMenuSceneName)
        {
            SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
        }
    }
}

