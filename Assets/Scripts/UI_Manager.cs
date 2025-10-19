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
    private GameObject UI_PickupText;
    [SerializeField]
    private GameObject UI_BoardWindowText;
    [SerializeField]
    private GameObject UI_NotEnoughPlanksText;
    [SerializeField]
    private GameObject UI_CarriedPlanksText;
    [SerializeField]
    private GameObject UI_TooHeavyText;

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManager;

    // Non-assignable variables
    private PlankManager plankManager;
    int currentPlanks;

    private void Awake()
    {
        plankManager = this.gameObject.GetComponent<PlankManager>();
    }
    void Start()
    {
        currentPlanks = this.gameObject.GetComponent<PlankManager>().HeldPlanks();
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + currentPlanks;
    }

    // Update is called once per frame
    void Update()
    {
        UI_PickupText.SetActive(raycastManager.LookingAtTag("Plank"));

        if (raycastManager.LookingAtTag("Window"))
        {
            WindowClass currentWindow = raycastManager.GetHitRecord().collider.gameObject.GetComponent<WindowClass>();

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

        UI_TooHeavyText.SetActive(currentPlanks >= 5);
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
}
