using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class MouseLook : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    private float cameraSensitivity = 0.2f;
    [SerializeField]
    private Vector2 cameraClampRangeX = new(-90f, 60f); // vertical

    [Header("Roots")]
    [Tooltip("Player Root for syncing rotation with player itself")]
    [SerializeField]
    private Transform playerRoot;

    [Tooltip("Reference to the Move action from the Input Actions asset.")]
    [Header("Input Action Reference")]
    [SerializeField]
    private InputActionReference rotationActionReference;

    // Non-assignable variables
    private Vector2 rotationInput;
    private float xRotation = 0f;
    private float yRotation = 0f;

    //private Camera playerCamera;

    private void Awake()
    {
        //playerCamera = gameObject.GetComponentInChildren<Camera>();
        //if (playerCamera == null)
        //{
        //    Debug.LogError("No Camera component is found in children!");
        //}
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        //playerCamera.enabled = true;
    }

    void Update()
    {
        if (!GameState.IsGameLost())
        {
            if (!GameState.IsGamePaused())
            {
                rotationInput = rotationActionReference.action.ReadValue<Vector2>() * cameraSensitivity;

                xRotation += rotationInput.y;
                xRotation = Mathf.Clamp(xRotation, cameraClampRangeX.x, cameraClampRangeX.y);

                yRotation += rotationInput.x;

                // UP/DOWN - rotate camera 
                transform.localRotation = Quaternion.Euler(-xRotation, 0f, 0f);
                // LEFT/RIGHT - rotate player root
                playerRoot.localRotation = Quaternion.Euler(0f, yRotation, 0f);
            }
        }
        else
        {
            //playerCamera.enabled = false;
        }
    }
}
