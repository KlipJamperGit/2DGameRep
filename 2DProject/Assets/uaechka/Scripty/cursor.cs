using UnityEngine;
using UnityEngine.SceneManagement;

public class cursor : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        print("Cursor unlocked");
    }

    // Update is called once per frame
    void Update()
    {
    }
}
