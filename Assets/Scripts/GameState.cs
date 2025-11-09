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

    // Script References
    private Freezing freezingRef;
    private DawnLogic dawnLogicRef;

    // Non-assignable variables
    private static bool gamePaused = false;
    private static bool gameLost = false;
    private static bool gameWon = false;

    [SerializeField]
    private float gameTimer = 0f;

    private void Awake()
    {
        if (freezingRef == null)
        {
            freezingRef = gameObject.GetComponent<Freezing>();
        }

        if (dawnLogicRef == null)
        {
            dawnLogicRef = gameObject.GetComponent<DawnLogic>();
        }
    }

    private void Start()
    {
        gameLost = false;
        gamePaused = false;
        gameWon = false;

        // Just in case they are not diabled from editor
        if (!gameplayViewPlane.activeSelf)
        {
            gameplayViewPlane.SetActive(true);
        }

        if (gameOverViewPlane.activeSelf)
        {
            gameOverViewPlane.SetActive(false);
        }
    }

    void Update()
    {
        if (!gameLost && !gameWon)
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

            gameTimer += Time.deltaTime;
            if (gameTimer >= 10f)
            {
                dawnLogicRef.UpdateDawn();
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

    public static bool IsGameWon()
    {
        return gameWon;
    }

    public static void SetGameWon()
    {
        gameWon = true;
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
