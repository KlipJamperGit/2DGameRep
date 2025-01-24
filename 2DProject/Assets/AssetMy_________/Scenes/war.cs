using UnityEngine;
using UnityEngine.SceneManagement;

public class war : MonoBehaviour
{
    [SerializeField] private string pauseMenuSceneName = "Pause Menu";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        SceneManager.UnloadSceneAsync(pauseMenuSceneName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
