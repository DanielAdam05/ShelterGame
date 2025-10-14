using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlankInteraction : MonoBehaviour
{
    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManager;

    [Header("Input action references")]
    [SerializeField]
    private InputActionReference interactActionReference;

    [Header("UI")]
    [SerializeField]
    private GameObject UI_pickupText;

    // Non-assignable variables
    private List<GameObject> inventoryPlanks = new();

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
        else if (UI_pickupText.activeSelf) UI_pickupText.SetActive(false);
    }
}
