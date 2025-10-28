using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField]
    private Transform cameraTransform;

    [Header("Movement Variables")]
    [SerializeField]
    private float moveSpeed = 2f;
    [SerializeField]
    private float sprintSpeed = 3.5f;
    [SerializeField]
    private float crouchSpeed = 0.5f;
    [SerializeField]
    private float verticalVelocity;

    [Header("Input Action References")]
    [SerializeField]
    private InputActionReference movementActionReference;
    [SerializeField]
    private InputActionReference jumpActionReference;
    [SerializeField]
    private InputActionReference crouchActionReference;
    [SerializeField]
    private InputActionReference sprintActionReference;

    [Header("Script Reference")]
    [SerializeField]
    private PlankManager plankManager;

    // Non-assignable variables
    private Vector3 playerDirection;
    private bool isCrouching = false;
    private CharacterController characterController;
    private float initialSpeed;

    private Vector2 moveInput;

    private void Awake()
    {
        initialSpeed = moveSpeed;
        if (characterController == null)
        {
            characterController = GetComponent<CharacterController>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.IsGamePaused())
        {
            MovePlayer();
        }
    }

    private void MovePlayer()
    {
        moveInput = movementActionReference.action.ReadValue<Vector2>();
        //playerDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical")).normalized;
        bool isCrouchHeld = crouchActionReference.action.ReadValue<float>() > 0.5f;
        bool isSprintHeld = sprintActionReference.action.ReadValue<float>() > 0.5f;

        Vector3 camForward = cameraTransform.forward;
        Vector3 camRight = cameraTransform.right;
        camForward.y = 0f;
        camRight.y = 0f;
        camForward.Normalize();
        camRight.Normalize();

        Vector3 desiredDirection = camForward * moveInput.y + camRight * moveInput.x;
        desiredDirection = desiredDirection.normalized;
       

        playerDirection = transform.TransformDirection(playerDirection);

        ApplyGravity(-20f);

        moveSpeed = initialSpeed;

        if (plankManager.HeldPlanks() < 5)
        {
            if (characterController.isGrounded)
            {
                isCrouching = false;
                characterController.height = 2f;

                if (isCrouchHeld)
                {
                    moveSpeed = crouchSpeed;

                    if ((characterController.height - 2f) <= 0.01f)
                    {
                        characterController.height = 1f;
                    }

                    isCrouching = true;
                }
                else if (isSprintHeld)
                {
                    moveSpeed = sprintSpeed;
                }

                //Jump();
            }
        }
        else
        {
            moveSpeed = crouchSpeed;
        }
        

        Vector3 playerMovement = moveSpeed * Time.deltaTime * desiredDirection;
        playerMovement.y = verticalVelocity * Time.deltaTime;

        characterController.Move(playerMovement);
    }

    private void ApplyGravity(float gravity)
    {
        if (!characterController.isGrounded)
        {
            verticalVelocity += gravity * Time.deltaTime;
        }
        else
        {
            verticalVelocity = -1f;
        }
    }

    private void Jump()
    {
        if (jumpActionReference.action.triggered && !isCrouching)
        {
            verticalVelocity = 5f;
        }
    }
}
