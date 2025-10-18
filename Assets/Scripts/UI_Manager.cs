using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class UI_Manager : MonoBehaviour
{
    [Header("UI Objects")]
    [SerializeField]
    private GameObject UI_pickupText;

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManager;

    [Header("Input action references")]
    [SerializeField]
    private InputActionReference interactActionReference;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (raycastManager.LookingAtPlank())
        {
            if (!UI_pickupText.activeSelf) UI_pickupText.SetActive(true);

            if (interactActionReference.action.triggered)
            {
                //raycastManager.GetRaycastHitObject().SetActive(false);
            }
        }
        else if (UI_pickupText.activeSelf)
            UI_pickupText.SetActive(false);
    }
}
