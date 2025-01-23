using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    [SerializeField] private string pauseMenuSceneName = "Pause Menu";
    private bool isPaused = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f; // Pause the game
            if (!SceneManager.GetSceneByName(pauseMenuSceneName).isLoaded)
            {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                SceneManager.LoadScene(pauseMenuSceneName, LoadSceneMode.Additive);
            }
        }
        else
        {
            Time.timeScale = 1f; // Resume the game
            if (SceneManager.GetSceneByName(pauseMenuSceneName).isLoaded)
            {
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                SceneManager.UnloadSceneAsync(pauseMenuSceneName);
            }

        }
    }
    public void LoadScene()
    {
        Time.timeScale = 1f; // Resume the game
        if (SceneManager.GetSceneByName(pauseMenuSceneName).isLoaded)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            SceneManager.UnloadSceneAsync(pauseMenuSceneName);
        }
    }
    public bool IsGamePaused()
    {
        return isPaused;
    }
}