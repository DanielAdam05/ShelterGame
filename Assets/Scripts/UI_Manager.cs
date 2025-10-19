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

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + this.gameObject.GetComponent<PlankManager>().HeldPlanks();
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
        UI_CarriedPlanksText.GetComponent<TextMeshProUGUI>().text = "x " + this.gameObject.GetComponent<PlankManager>().HeldPlanks();
    }
}
