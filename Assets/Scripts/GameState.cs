using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [SerializeField]
    private InputActionReference pauseAction;

    [Header("Object dependent on GameState")]
    [Tooltip("Objects whose lifetime is managed by Game State")]
    [SerializeField]
    private AudioSource windSound;

    [Space(10)]
    [SerializeField]
    private GameObject gameplayViewPlane;
    [SerializeField]
    private GameObject gameOverViewPlane;

    // Non-assignable variables
    private static bool gamePaused = false;
    private static bool gameLost = false;

    // Script References
    private Freezing freezingRef;

    private void Awake()
    {
        if (freezingRef == null)
        {
            freezingRef = gameObject.GetComponent<Freezing>();
        }
    }

    private void Start()
    {
        gameLost = false;
        gamePaused = false;

        if (!gameplayViewPlane.activeSelf)
        {
            gameplayViewPlane.SetActive(true);
        }

        // just in case they are not diabled from editor
        
        if (gameOverViewPlane.activeSelf)
        {
            gameOverViewPlane.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameLost)
        {
            if (freezingRef.FrozeToDeath()) // lose conditions check
            {
                gameLost = true;

                Cursor.lockState = CursorLockMode.None;

                if (gameplayViewPlane.activeSelf)
                    gameplayViewPlane.SetActive(false);

                if (!gameOverViewPlane.activeSelf)
                    gameOverViewPlane.SetActive(true);
            }
            else if (pauseAction.action.triggered) // (un)pause if game is not lost
            {
                ChangePauseState();
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

    public static bool IsGameLost()
    {
        return gameLost;
    }

    public void QuitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void ChangePauseState()
    {
        if (gamePaused)
        {
            gamePaused = false;
            Cursor.lockState = CursorLockMode.Locked;

            if (windSound.mute)
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
}
