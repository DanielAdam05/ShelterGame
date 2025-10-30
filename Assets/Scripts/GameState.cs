using UnityEngine;
using UnityEngine.InputSystem;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pauseAction;

    [SerializeField]
    private AudioSource windSound;

    // Non-assignable variables
    private static bool gamePaused;

    void Update()
    {
        if (pauseAction.action.triggered)
        {
            if (gamePaused)
            {
                gamePaused = false;
                Cursor.lockState = CursorLockMode.Locked;

                if(windSound.mute)
                    windSound.mute = false;
            }
            else
            {
                gamePaused = true;
                Cursor.lockState = CursorLockMode.None;

                if (!windSound.mute)
                    windSound.mute = true;
            }
            
        }

        if (gamePaused)
        {
            if (Time.timeScale != 0f)
                Time.timeScale = 0f;
        }
        else
        {
            if (Time.timeScale != 1f)
                Time.timeScale = 1f;
        } 
    }

    public static bool IsGamePaused()
    {
        return gamePaused;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
