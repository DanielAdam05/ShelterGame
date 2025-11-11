using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [Header("Major UI Parts")]
    [SerializeField]
    private GameObject PlayerUI;
    [SerializeField]
    private GameObject PauseScreen;
    [SerializeField]
    private GameObject GameOverScreen;
    [SerializeField]
    private GameObject NoteUIScreen;


    [Header("UI Objects")]
    [SerializeField]
    private GameObject UI_Crosshair;
    [SerializeField]
    private GameObject UI_PickupText;
    [SerializeField]
    private GameObject UI_BoardWindowText;
    [SerializeField]
    private GameObject UI_NotEnoughPlanksText;
    [SerializeField]
    private GameObject UI_CarriedPlanksText;
    [SerializeField]
    private GameObject UI_TooHeavyText;
    [SerializeField]
    private GameObject UI_ExtendFireText;
    [SerializeField]
    private RawImage UI_FreezeEffectImage;
    [SerializeField]
    private GameObject UI_FeelSafeText;
    [SerializeField]
    private GameObject UI_OutsideDoorText;
    [SerializeField]
    private GameObject UI_ReadNoteText;

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManagerReference;
    [SerializeField]
    private Freezing freezingReference;
    [SerializeField]
    private NoteInteraction noteInteractionReference;
    [SerializeField]
    private PlankManager plankManager;

    // Non-assignable variables
    int currentPlanks;
    private TextMeshProUGUI feelSafeTextMeshProUGUI;
    private TextMeshProUGUI outsideDootTextMeshProUGUI;

    private void Awake()
    {
        if (plankManager == null)
        {
            plankManager = gameObject.GetComponent<PlankManager>();
        }

        if (UI_FeelSafeText != null)
        {
            feelSafeTextMeshProUGUI = UI_FeelSafeText.GetComponent<TextMeshProUGUI>();
        }

        if (UI_OutsideDoorText != null)
        {
            outsideDootTextMeshProUGUI = UI_OutsideDoorText.GetComponent<TextMeshProUGUI>();
        }
    }
    void Start()
    {
        if (plankManager != null)
        {
            currentPlanks = plankManager.HeldPlanks();
        }
        
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + currentPlanks;

        if (GameOverScreen.activeSelf)
        {
            GameOverScreen.SetActive(false);
        }
    }

    void Update()
    {
        bool isGamePaused = GameState.IsGamePaused();

        if (!GameState.IsGameLost())
        {
            if (!isGamePaused)
            {
                UpdateFreezeImageAlpha();

                UI_PickupText.SetActive(raycastManagerReference.LookingAtTag("Plank") || raycastManagerReference.LookingAtTag("Lighter"));

                ManageWindowBoardingUI();

                UI_ExtendFireText.SetActive(raycastManagerReference.LookingAtTag("Fireplace"));

                UI_TooHeavyText.SetActive(currentPlanks >= 5);

                UI_ReadNoteText.SetActive(noteInteractionReference.GetLookingAtNote() && !noteInteractionReference.GetNoteOpen());
                NoteUIScreen.SetActive(noteInteractionReference.GetNoteOpen());
            }

            // Pause
            PauseScreen.SetActive(isGamePaused);
            UI_Crosshair.SetActive(!isGamePaused);
        }
        else
        {
            GameOverScreen.SetActive(true);

            PlayerUI.SetActive(false);

            // if try again button is pressed -> call GameState.ResetScene()
        } 
    }

    public void ShowNotEnoughPlanksText(float duration = 2f)
    {
        UI_NotEnoughPlanksText.SetActive(true);
        StopAllCoroutines();
        StartCoroutine(DisableObject(UI_NotEnoughPlanksText, duration));
    }

    private IEnumerator DisableObject(GameObject objectToDisable, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        objectToDisable.SetActive(false);
    }

    public void UpdateHeldPlankNumber()
    {
        currentPlanks = plankManager.HeldPlanks();
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + currentPlanks;
    }

    private void ManageWindowBoardingUI()
    {
        if (raycastManagerReference.LookingAtTag("Window") || raycastManagerReference.LookingAtTag("SmallWindow"))
        {
            WindowClass currentWindow = raycastManagerReference.GetHitRecord().collider.gameObject.GetComponent<WindowClass>();

            if (currentWindow != null && !currentWindow.IsWindowBoarded())
            {
                UI_BoardWindowText.SetActive(true);
            }
            else UI_BoardWindowText.SetActive(false);
        }
        else
        {
            UI_BoardWindowText.SetActive(false);
        }
    }

    private void UpdateFreezeImageAlpha()
    {
        Color color = UI_FreezeEffectImage.color;
        color.a = freezingReference.FreezeMeter() / 400f;

        UI_FreezeEffectImage.color = color;
    }

    public IEnumerator PlayYouFeelSafeText(float duration = 1.5f)
    {
        UI_FeelSafeText.SetActive(true);

        Color originalColor = new(feelSafeTextMeshProUGUI.color.r, feelSafeTextMeshProUGUI.color.g, feelSafeTextMeshProUGUI.color.b, 1f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            feelSafeTextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure alpha is exactly zero at end
        feelSafeTextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        UI_FeelSafeText.SetActive(false);
    }

    public IEnumerator PlayOutsideDoorText(float duration = 1.5f)
    {
        UI_OutsideDoorText.SetActive(true);

        Color originalColor = new(outsideDootTextMeshProUGUI.color.r, outsideDootTextMeshProUGUI.color.g, outsideDootTextMeshProUGUI.color.b, 1f);
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float alpha = Mathf.Lerp(1f, 0f, elapsed / duration);
            outsideDootTextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, alpha);
            yield return null;
        }

        // Ensure alpha is exactly zero at end
        outsideDootTextMeshProUGUI.color = new Color(originalColor.r, originalColor.g, originalColor.b, 0f);
        UI_OutsideDoorText.SetActive(false);
    }
}
