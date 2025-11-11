using UnityEngine;

[RequireComponent(typeof(PositionFollower))]
public class ViewBobbing : MonoBehaviour
{
    [Header("View Bobbing Variables")]
    [SerializeField]
    private float effectIntensity;
    [SerializeField]
    private float effectIntesityX;
    [SerializeField]
    private float effectSpeed;

    [Header("Script Reference")]
    [SerializeField]
    private PlayerMovement playerMovementRef;

    // Non-assignable variables
    private PositionFollower positionFollowerRef;
    private Vector3 originalOffset;
    private float sinTime;

    private void Awake()
    {
        positionFollowerRef = GetComponent<PositionFollower>();
    }
    void Start()
    {
        originalOffset = positionFollowerRef.offset;
    }

    void Update()
    {
        if (!GameState.IsGamePaused() && !GameState.IsGameWon() && !GameState.IsGameLost())
        {
            if (playerMovementRef.IsMoving())
            {
                sinTime += effectSpeed * Time.deltaTime;
            }
            else
            {
                sinTime = 0f;
            }

            float sinAmountY = -Mathf.Abs(effectIntensity * Mathf.Sin(sinTime));
            Vector3 sinAmountX = effectIntensity * effectIntesityX * Mathf.Cos(sinTime) * positionFollowerRef.transform.right;

            positionFollowerRef.offset = new Vector3(originalOffset.x, effectIntensity * sinAmountY, originalOffset.z);
            positionFollowerRef.offset += sinAmountX;
        }
    }
}
