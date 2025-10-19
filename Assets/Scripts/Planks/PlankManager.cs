using NUnit.Framework;
using System.Collections.Generic;
using UnityEditor.PackageManager.UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;
using UnityEngine.Rendering;

public class PlankManager : MonoBehaviour
{
    [Header("Worldspace Plank Locations")]
    [SerializeField]
    private List<GameObject> levelPlanks;

    [Space(10)]
    [SerializeField]
    private int activatedPlanksOnStart = 10;
    [Space(5)]
    [SerializeField]
    private int heldPlanks = 3;

    [Header("Input action references")]
    [SerializeField]
    private InputActionReference interactActionReference;

    [Header("Script references")]
    [SerializeField]
    private RaycastManager raycastManager;

    // Non-assignable variables


    void Start()
    {
        //Debug.Log("Carried planks: " + heldPlanks);
        EnableRandomPlanks(levelPlanks, activatedPlanksOnStart);
    }

    private void OnEnable()
    {
        interactActionReference.action.performed += OnInteractPerformed;
        interactActionReference.action.Enable();
    }
    private void OnDisable()
    {
        interactActionReference.action.performed -= OnInteractPerformed;
        interactActionReference.action.Disable();
    }

    void Update()
    {
        
    }

    private void OnInteractPerformed(InputAction.CallbackContext context)
    {
        if (context.interaction is PressInteraction)
        {
            //Debug.Log("Pressed E");

            if (raycastManager.LookingAtTag("Plank"))
            {
                GetPlank();
                this.gameObject.GetComponent<UI_Manager>().UpdateHeldPlankNumber();
            }
            else if (raycastManager.LookingAtTag("Window") && heldPlanks < 3)
            {
                WindowClass currentWindow = raycastManager.GetHitRecord().collider.gameObject.GetComponent<WindowClass>();

                if (!currentWindow.IsWindowBoarded())
                {
                    this.gameObject.GetComponent<UI_Manager>().ShowNotEnoughPlanksText(2f);
                }
            }
        }
        else if (context.interaction is HoldInteraction)
        {
            //Debug.Log("Held E");

            if (raycastManager.LookingAtTag("Window"))
            {
                WindowClass currentWindow = raycastManager.GetHitRecord().collider.gameObject.GetComponent<WindowClass>();
                if (heldPlanks < 3)
                {
                    if (!currentWindow.IsWindowBoarded())
                    {
                        this.gameObject.GetComponent<UI_Manager>().ShowNotEnoughPlanksText(2f);
                    }
                }
                else
                {
                    //Debug.Log("Used 3 planks to board window");
                    heldPlanks -= 3;
                    this.gameObject.GetComponent<UI_Manager>().UpdateHeldPlankNumber();

                    // spawn planks around window logic
                    if (currentWindow != null && !currentWindow.IsWindowBoarded())
                    {
                        currentWindow.BoardWindow();
                    }
                }
            }
        }
    }

    private void EnableRandomPlanks(List<GameObject> container, int count)
    {
        List<GameObject> tempList = new(container);
        int startSize = tempList.Count;

        //Debug.Log("Spawned planks: " + startSize);

        for (int i = 0; i < count; ++i) // loop to randomize planks [count] times
        {
            int randomIdx = Random.Range(i, startSize);

            (tempList[randomIdx], tempList[i]) = (tempList[i], tempList[randomIdx]);
        }

        for (int i = 0; i < count; ++i) // activate the planks
        {
            tempList[i].SetActive(true);
        }
    }

    private void GetPlank()
    {
        //Debug.Log("Got 1 plank");

        raycastManager.GetHitRecord().collider.gameObject.SetActive(false);

        ++heldPlanks;
    }

    public int HeldPlanks() 
    {
        return heldPlanks;
    }
}
