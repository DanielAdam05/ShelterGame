using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
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
    private GameObject UI_PauseScreen;

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManagerReference;
    [SerializeField]
    private Freezing freezingReference;

    // Non-assignable variables
    private PlankManager plankManager;
    int currentPlanks;

    private void Awake()
    {
        if (plankManager == null)
        {
            plankManager = gameObject.GetComponent<PlankManager>();
        }
    }
    void Start()
    {
        if (plankManager != null)
        {
            currentPlanks = gameObject.GetComponent<PlankManager>().HeldPlanks();
        }
        
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + currentPlanks;
    }

    void Update()
    {
        if (!GameState.IsGamePaused())
        {
            CalculateFreezeImageAlpha();

            UI_PickupText.SetActive(raycastManagerReference.LookingAtTag("Plank") || raycastManagerReference.LookingAtTag("Lighter"));

            ManageWindowBoardingUI();

            UI_ExtendFireText.SetActive(raycastManagerReference.LookingAtTag("Fireplace"));

            UI_TooHeavyText.SetActive(currentPlanks >= 5);
        }

        // Pause
        UI_PauseScreen.SetActive(GameState.IsGamePaused());
        UI_Crosshair.SetActive(!GameState.IsGamePaused());
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
        if (raycastManagerReference.LookingAtTag("Window"))
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

    private void CalculateFreezeImageAlpha()
    {
        Color color = UI_FreezeEffectImage.color;
        color.a = freezingReference.FreezeMeter() / 400f;

        UI_FreezeEffectImage.color = color;
    }
}
