using UnityEngine;
using UnityEngine.InputSystem;

public class DoorInteraction : MonoBehaviour
{
    [Header("Open door Input")]
    [SerializeField]
    private InputActionReference doorActionReference;

    [Header("Script Refrences")]
    [SerializeField]
    private RaycastManager raycastManager;
    [SerializeField]
    private UI_Manager uiManager;

    // Non-assignable variables

    void Update()
    {
        if (!GameState.IsGamePaused() && !GameState.IsGameWon() && !GameState.IsGameLost())
        {
            if (doorActionReference.action.triggered)
            {
                if (raycastManager.LookingAtTag("Door"))
                {
                    StartCoroutine(uiManager.PlayOutsideDoorText());
                }
            }
        }
    }
}
