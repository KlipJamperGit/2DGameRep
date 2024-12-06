using UnityEngine;
using UnityEngine.SceneManagement;

public class IsOutOfMap : MonoBehaviour
{
    // Name of the pause menu scene
    [SerializeField] private string pauseMenuSceneName = "Main-Menu-Example";

    private void OnCollisionEnter(Collision collision)
    {
        LoadPauseMenu();
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


