using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

public class EnemyAttackCheck : MonoBehaviour
{
    [Header("Enemy Variables")]
    [SerializeField]
    private float shadowRunSpeed;
    [SerializeField]
    private float maxTimeWithoutBeingSeen = 10f;

    [Header("Transform Variables")]
    [SerializeField]
    private Transform outsideWarmRoomPos;
    [SerializeField]
    private Transform insideWarmRoomPos;

    [Space(10)]
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private Transform playerTransform;

    [Header("Script Refrences")]
    [SerializeField]
    private Freezing freezingRef;

    [Space(10)]
    [SerializeField]
    private RenderTexture gameplayRenderTexture;

    [Header("Enemy Raycast Layermask")]
    [SerializeField]
    private LayerMask lineOfSightLayerMask;

    // Non-assignable variables
    private Transform playerCameraTransform;

    [Space(10)]
    [SerializeField]
    private bool attackSequenceStarted = false;
    private float attackTimer = 0f;
    private Vector3 offsetShadowWorldPos;

    private float screenWidth;
    private float screenHeight;
    private bool lineOfSightClear;
    private Vector3 shadowScreenPos;

    private AudioSource jumpScareSound;
    private bool canPlayJumpScareSound = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerCameraTransform = playerCamera.transform;

        attackSequenceStarted = false;
        screenWidth = gameplayRenderTexture.width;
        screenHeight = gameplayRenderTexture.height;

        offsetShadowWorldPos = gameObject.transform.position + new Vector3(0f, 0.4f, 0f);
        shadowScreenPos = playerCamera.WorldToScreenPoint(offsetShadowWorldPos);

        jumpScareSound = gameObject.GetComponent<AudioSource>();
        canPlayJumpScareSound = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GameState.IsGamePaused() && !GameState.IsGameWon() && !GameState.IsGameLost())
        {
            if (attackSequenceStarted)
            {
                attackTimer += Time.deltaTime;

                offsetShadowWorldPos = gameObject.transform.position + new Vector3(0f, 0.4f, 0f);
                shadowScreenPos = playerCamera.WorldToScreenPoint(offsetShadowWorldPos);
                if (EnemyOnScreen(screenWidth, screenHeight))
                {
                    RushPlayer();
                }
                else if (attackTimer >= maxTimeWithoutBeingSeen)
                {
                    RushPlayer();
                }
            }
        }
    }

    public IEnumerator StartAttackSequence()
    {
        attackSequenceStarted = true;
        yield return new WaitForSeconds(5f);
        SpawnShadowEnemy();
    }

    private void SpawnShadowEnemy()
    {
        if (freezingRef.IsPlayerInWarmRoom())
        {
            this.gameObject.transform.SetPositionAndRotation(outsideWarmRoomPos.position, outsideWarmRoomPos.rotation);
        }
        else
        {
            this.gameObject.transform.SetPositionAndRotation(insideWarmRoomPos.position, insideWarmRoomPos.rotation);
        }
    }

    private void RushPlayer()
    {
        if (canPlayJumpScareSound)
        {
            jumpScareSound.Play();
            canPlayJumpScareSound = false;
        }

        Vector3 enemyToPlayerDir = playerCameraTransform.position - gameObject.transform.position;
        gameObject.transform.position += shadowRunSpeed * Time.deltaTime * enemyToPlayerDir;
    }

    private bool EnemyOnScreen(float screenWidth, float screenHeight)
    {
        Vector3 enemyToPlayerVector = offsetShadowWorldPos - playerCameraTransform.position;

        if (Physics.Raycast(playerCameraTransform.position, enemyToPlayerVector.normalized, out RaycastHit hit, enemyToPlayerVector.magnitude, lineOfSightLayerMask))
        {
            lineOfSightClear = false;
        }
        else
        {
            lineOfSightClear = true;
        }

        return (shadowScreenPos.x > 0 && shadowScreenPos.x <= screenWidth) &&
                (shadowScreenPos.y > 0 && shadowScreenPos.y <= screenHeight) &&
                shadowScreenPos.z >= 0 &&
                lineOfSightClear;
    }

    public bool AttackSequenceStarted()
    {
        return attackSequenceStarted;
    }

    public bool ShadowCollidedWithPlayer()
    {
        float distance = Vector3.Distance(gameObject.transform.position, playerTransform.position);
        return distance <= 1.5f;
    }
}
