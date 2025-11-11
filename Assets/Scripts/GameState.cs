using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class GameState : MonoBehaviour
{
    [Header("Camera Pivot Variables")]
    [SerializeField]
    private Transform cameraPivot;
    [SerializeField]
    private Transform gameLostCameraPos;
    [SerializeField]
    private Transform gameWonCameraPos;

    [Header("Pause Input")]
    [SerializeField]
    private InputActionReference pauseAction;

    [Tooltip("Sounds lifetime is managed by Game State")]
    [Header("Objects dependent on GameState")]
    [SerializeField]
    private AudioSource windSound;
    [SerializeField]
    private AudioSource shadowShoutSound;
    [SerializeField]
    private ParticleSystem snow1;
    [SerializeField]
    private ParticleSystem snow2;

    // Script References
    private Freezing freezingRef;
    private DawnLogic dawnLogicRef;

    // Non-assignable variables
    private static bool gamePaused = false;
    private static bool gameLost = false;
    private static bool gameWon = false;

    [Space(5)]
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
    }

    void Update()
    {
        if (!gameLost)
        {
            if (!gameWon)
            {
                if (freezingRef.FrozeToDeath()) // lose conditions check
                {
                    gameLost = true;

                    Cursor.lockState = CursorLockMode.None;

                    cameraPivot.transform.SetPositionAndRotation(gameLostCameraPos.transform.position, gameLostCameraPos.transform.rotation);
                }
                else if (pauseAction.action.triggered) // (un)pause if game is not lost
                {
                    ChangePauseState();
                }

                gameTimer += Time.deltaTime;
                if (gameTimer >= dawnLogicRef.GetDawnStart())
                {
                    dawnLogicRef.UpdateDawn();
                }
            }
            else
            {
                Cursor.lockState = CursorLockMode.None;

                cameraPivot.transform.SetPositionAndRotation(gameWonCameraPos.transform.position, gameWonCameraPos.transform.rotation);

                snow1.Stop();
                snow2.Stop();
                windSound.volume = 0.04f;
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

            if(!shadowShoutSound.isPlaying)
                shadowShoutSound.UnPause();
        }
        else
        {
            gamePaused = true;
            Cursor.lockState = CursorLockMode.None;

            if (!windSound.mute)
                windSound.mute = true;

            if (shadowShoutSound.isPlaying)
                shadowShoutSound.Pause();
        }
    }
}
