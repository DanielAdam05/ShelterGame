using UnityEngine;
using UnityEngine.InputSystem;

public class NoteInteraction : MonoBehaviour
{
    [Header("Read Note Input")]
    [SerializeField]
    private InputActionReference noteActionReference;

    [Header("Script Refrences")]
    [SerializeField]
    private RaycastManager raycastManager;
    [SerializeField]
    private MouseLook mouseLook;
    [SerializeField]
    private PlayerMovement playerMovement;

    // Non-assignable variables
    private bool isNoteOpen = false;
    private bool isLookingAtNote = false;

    void Start()
    {
        isNoteOpen = false;
    }

    void Update()
    {
        if (raycastManager.LookingAtTag("Note"))
        {
            isLookingAtNote = true;

            if (noteActionReference.action.triggered)
            {
                isNoteOpen = !isNoteOpen;

                mouseLook.enabled = !mouseLook.enabled;
                playerMovement.enabled = !playerMovement.enabled;
            }
        }
        else
        {
            isLookingAtNote = false;
        }
    }

    public bool GetNoteOpen()
    {
        return isNoteOpen;
    }

    public bool GetLookingAtNote()
    { 
        return isLookingAtNote;
    }
}
